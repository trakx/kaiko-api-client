using KaikoSdk.Stream.MarketUpdateV1;
using static KaikoSdk.StreamMarketUpdateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class MarketUpdateClientTests
{
    private MarketUpdateRequest _request;
    private readonly StreamMarketUpdateServiceV1Client _sdkClient;
    private readonly Func<IAsyncEnumerable<MarketUpdateResponse>> _streamAction;

    public MarketUpdateClientTests()
    {
        _request = new MarketUpdateRequest
        {
            Exchanges = new[] { "Trakx" },
            BaseSymbols = new[] { "btc", "eth" },
            QuoteSymbols = new[] { "usd" },
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

    private void SetupSubscribeResponse()
    {
        var response = new AsyncServerStreamingCall<StreamMarketUpdateResponseV1>(
                     new TestMarketUpdateStreamReader(),
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
    public StreamMarketUpdateResponseV1 Current => new();

    public Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }
}