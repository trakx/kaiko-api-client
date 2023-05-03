using Google.Protobuf.WellKnownTypes;
using KaikoSdk.Core;
using KaikoSdk.Stream.MarketUpdateV1;
using static KaikoSdk.StreamMarketUpdateServiceV1;
using SdkUpdateType = KaikoSdk.Stream.MarketUpdateV1.StreamMarketUpdateResponseV1.Types.StreamMarketUpdateType;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class MarketUpdateClientTests
{
    private const string ExchangeCode = "trakx";
    private const string QuoteSymbol = "usd";
    private const string BaseSymbol1 = "abc";
    private const string BaseSymbol2 = "xyz";

    private MarketUpdateRequest _request;
    private readonly StreamMarketUpdateServiceV1Client _sdkClient;
    private readonly Func<IAsyncEnumerable<MarketUpdateResponse>> _streamAction;

    public MarketUpdateClientTests()
    {
        _request = new MarketUpdateRequest
        {
            Exchanges = new[] { ExchangeCode },
            BaseSymbols = new[] { BaseSymbol1, BaseSymbol2 },
            QuoteSymbols = new[] { QuoteSymbol },
            IncludeTopOfBook = true,
        };

        _sdkClient = Substitute.For<StreamMarketUpdateServiceV1Client>();

        var client = new MarketUpdateClient(_sdkClient);

        _streamAction = () => client.Stream(_request);
    }

    [Fact]
    public void StreamAsync_expects_request()
    {
        _request = null!;
        _streamAction.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("request");
    }

    [Fact]
    public async Task Empty_exchanges_in_StreamAsync_requests_all_exchanges_from_sdk()
    {
        SetupSubscribeResponse();

        _request.Exchanges = Array.Empty<string>();

        var action = async () => await _streamAction().AnyAsync();

        await action.Should().NotThrowAsync();

        var sdkCall = _sdkClient
            .ReceivedCalls()
            .FirstOrDefault(p => p.GetMethodInfo().Name == nameof(_sdkClient.Subscribe));

        sdkCall.Should().NotBeNull();

        var sdkRequest = sdkCall!
            .GetArguments()
            .FirstOrDefault(p => p is StreamMarketUpdateRequestV1)
            as StreamMarketUpdateRequestV1;

        sdkRequest.Should().NotBeNull();

        sdkRequest!.InstrumentCriteria.Exchange.Should().Be(MarketUpdateClient.AllExchangesWildcard);
    }

    [Fact]
    public void StreamAsync_expects_base_symbol()
    {
        _request.BaseSymbols = Array.Empty<string>();
        _streamAction.Should()
            .Throw<ArgumentException>()
            .WithMessage(MarketUpdateClient.MissingBaseSymbolsError);
    }

    [Fact]
    public void StreamAsync_expects_quote_symbol()
    {
        _request.QuoteSymbols = Array.Empty<string>();
        _streamAction.Should()
            .Throw<ArgumentException>()
            .WithMessage(MarketUpdateClient.MissingQuoteSymbolsError);
    }

    [Theory]
    [InlineData(false, false, false, true)]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    [InlineData(true, true, true)]
    public void StreamAsync_expects_at_least_one_flag(bool topBook, bool fullBook, bool includeTrades, bool expectException = false)
    {
        _request.IncludeTopOfBook = topBook;
        _request.IncludeFullBook = fullBook;
        _request.IncludeTrades = includeTrades;

        if (!expectException)
        {
            _streamAction.Should().NotThrow();
            return;
        }

        _streamAction.Should()
            .Throw<ArgumentException>()
            .WithMessage(MarketUpdateClient.MissingDataTypeError);
    }

    [Fact]
    public async Task StreamAsync_converts_responses_to_MarketUpdateResponse()
    {
        var sdkResponse1 = SetupSdkResponse(BaseSymbol1, SdkUpdateType.BestAsk, 23, 0.1);
        var sdkResponse2 = SetupSdkResponse(BaseSymbol1, SdkUpdateType.BestBid, 25, 0.2);

        SetupSubscribeResponse(sdkResponse1, sdkResponse2);

        var list = await _streamAction().ToListAsync();

        list.Should().HaveCount(2);
        list.Should().SatisfyRespectively(
            clientResponse1 => CompareResponses(clientResponse1, sdkResponse1),
            clientResponse2 => CompareResponses(clientResponse2, sdkResponse2));
    }

    private static StreamMarketUpdateResponseV1 SetupSdkResponse(string baseSymbol, SdkUpdateType updateType, double price, double amount)
    {
        var exchangeTime = new Timestamp { Seconds = 1_680_000_000, Nanos = 1234 };
        var collectionTime = exchangeTime + new Duration { Seconds = 2 };
        var eventTime = exchangeTime + new Duration { Seconds = 5 };

        return new StreamMarketUpdateResponseV1
        {
            Commodity = StreamMarketUpdateCommodity.SmucTopOfBook,
            Exchange = ExchangeCode,
            Code = $"{baseSymbol}-{QuoteSymbol}",
            UpdateType = updateType,
            Amount = amount,
            Price = price,
            SequenceId = Guid.NewGuid().ToString(),
            TsEvent = eventTime,
            TsCollection = new TimestampValue { Value = collectionTime },
            TsExchange = new TimestampValue { Value = exchangeTime },
        };
    }

    private static void CompareResponses(MarketUpdateResponse clientResponse, StreamMarketUpdateResponseV1 sdkResponse)
    {
        var sdkSymbols = sdkResponse.Code.Split('-');
        clientResponse.BaseSymbol.Should().Be(sdkSymbols[0]);
        clientResponse.QuoteSymbol.Should().Be(sdkSymbols[1]);

        clientResponse.Exchange.Should().Be(sdkResponse.Exchange);
        clientResponse.Price.Should().Be((decimal)sdkResponse.Price);
        clientResponse.Amount.Should().Be((decimal)sdkResponse.Amount);

        clientResponse.Timestamp.Should().Be(sdkResponse.TsEvent.ToDateTimeOffset());
        clientResponse.TimestampExchange.Should().Be(sdkResponse.TsExchange.Value.ToDateTimeOffset());
        clientResponse.TimestampCollection.Should().Be(sdkResponse.TsCollection.Value.ToDateTimeOffset());

        clientResponse.UpdateType.ToString().Should().Be(sdkResponse.UpdateType.ToString());
        clientResponse.SequenceId.Should().Be(sdkResponse.SequenceId);
    }

    private void SetupSubscribeResponse(params StreamMarketUpdateResponseV1[] responses)
    {
        var response = new AsyncServerStreamingCall<StreamMarketUpdateResponseV1>(
                     new TestMarketUpdateStreamReader(responses),
                     Task.FromResult(new Metadata()),
                     () => throw new NotImplementedException(),
                     () => throw new NotImplementedException(),
                     () => { });

        _sdkClient
            .Subscribe(Arg.Any<StreamMarketUpdateRequestV1>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(response);
    }
}

public class TestMarketUpdateStreamReader : IAsyncStreamReader<StreamMarketUpdateResponseV1>
{
    private int _currentIndex;
    private readonly StreamMarketUpdateResponseV1[] _responses;

    public StreamMarketUpdateResponseV1 Current => _responses.ElementAtOrDefault(_currentIndex) ?? new();

    public TestMarketUpdateStreamReader(StreamMarketUpdateResponseV1[] responses)
    {
        _responses = responses;
        _currentIndex = -1;
    }

    public Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        _currentIndex++;
        var hasNext = _currentIndex < _responses.Length;
        return Task.FromResult(hasNext);
    }
}