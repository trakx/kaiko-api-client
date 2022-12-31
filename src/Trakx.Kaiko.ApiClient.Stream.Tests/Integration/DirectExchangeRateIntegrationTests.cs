using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class DirectExchangeRateIntegrationTests : ExchangeRateIntegrationTestsBase<IDirectExchangeRatesClient>
{
    public DirectExchangeRateIntegrationTests(KaikoStreamFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Theory]
    [InlineData("btc")]
    [InlineData("eth")]
    public async Task Stream_should_return_prices(string symbol, string currency = "usd")
    {
        var replies = await StreamAsync(symbol, currency, StatusCode.Cancelled);
        replies.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("btc")]
    [InlineData("eth")]
    public async Task Observable_should_return_prices(string symbol, string currency = "usd")
    {
        var replies = await ObserveAsync(symbol, currency, StatusCode.Cancelled);
        replies.Should().BeGreaterThan(0);
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
