using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using Microsoft.Reactive.Testing;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class SpotExchangeRateTests : ExchangeRateClientTestsBase
{
    private readonly ISpotExchangeRatesClient _client;

    public SpotExchangeRateTests(KaikoStreamFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _client = fixture.Services.GetRequiredService<ISpotExchangeRatesClient>();
    }

    [Theory]
    [InlineData("btc")]
    [InlineData("eth")]
    public async Task Stream_should_return_prices(string symbol, string currency = "usd")
    {
        const int seconds = 2;
        var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

        var replies = 0;
        try
        {
            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);

            await foreach (var response in _client.Stream(request, cancellation.Token))
            {
                AssertResponse(response, symbol, currency);
                replies++;
            }
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().Be(StatusCode.Cancelled);
        }

        replies.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("btc")]
    [InlineData("eth")]
    public async Task Observable_should_return_prices(string symbol, string currency = "usd")
    {
        const int seconds = 2;
        var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

        var replies = 0;

        try
        {
            void onNext(ExchangeRateResponse response)
            {
                AssertResponse(response, symbol, currency);
                replies++;
            }

            void onError(Exception x)
            {
                Output.WriteLine(x.Message);
            }

            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);
            await _client
                .Observe(request, cancellation.Token)
                .Do(onNext, onError)
                .LastOrDefaultAsync();
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().Be(StatusCode.Cancelled);
        }

        replies.Should().BeGreaterThan(0);
    }
}