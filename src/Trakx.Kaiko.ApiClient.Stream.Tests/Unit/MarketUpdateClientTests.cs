﻿using static KaikoSdk.StreamMarketUpdateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class MarketUpdateClientTests
{
    private MarketUpdateRequest _request;
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

        var sdkClient = Substitute.For<StreamMarketUpdateServiceV1Client>();
        var client = new MarketUpdateClient(sdkClient);

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
    public void StreamAsync_expects_exchanges()
    {
        _request.Exchanges = Array.Empty<string>();
        _streamAction.Should()
            .Throw<ArgumentException>()
            .WithMessage(MarketUpdateClient.MissingExchangesError);
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
}