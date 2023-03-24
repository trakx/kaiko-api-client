using System.Reactive.Linq;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class DirectExchangeRateIntegrationTests : ExchangeRateIntegrationTestsBase<IDirectExchangeRatesClient>
{
    public DirectExchangeRateIntegrationTests(KaikoStreamFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Theory]
    [InlineData(EnabledServices.SpotExchangeRate, "btc")]
    [InlineData(EnabledServices.SpotExchangeRate, "eth")]
    public async Task Stream_should_return_prices(bool serviceEnabled, string symbol, string currency = "usd")
    {
        var replies = await StreamAsync(symbol, currency, StatusCode.Cancelled);
        AssertReplies(serviceEnabled, replies);
    }

    [Theory]
    [InlineData(EnabledServices.SpotExchangeRate, "btc")]
    [InlineData(EnabledServices.SpotExchangeRate, "eth")]
    public async Task Observable_should_return_prices(bool serviceEnabled, string symbol, string currency = "usd")
    {
        var replies = await ObserveAsync(symbol, currency, StatusCode.Cancelled);
        AssertReplies(serviceEnabled, replies);
    }

    [Fact]
    public async Task Stream_should_handle_unknown_symbol()
    {
        var replies = await StreamAsync("mani", "usd", StatusCode.FailedPrecondition);
        replies.Should().Be(0);
    }

    [Fact]
    public async Task Stream_should_handle_unknown_currency()
    {
        var replies = await StreamAsync("btc", "mani", StatusCode.FailedPrecondition);
        replies.Should().Be(0);
    }

    [Fact]
    public async Task Observe_should_handle_unknown_symbol()
    {
        var replies = await ObserveAsync("mani", "usd", StatusCode.FailedPrecondition);
        replies.Should().Be(0);
    }

    [Fact]
    public async Task Observe_should_handle_unknown_currency()
    {
        var replies = await ObserveAsync("btc", "mani", StatusCode.FailedPrecondition);
        replies.Should().Be(0);
    }
}
