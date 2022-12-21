using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using KaikoSdk.Stream.AggregatesDirectExchangeRateV1;
using Microsoft.Reactive.Testing;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class DirectExchangeRateTests : ExchangeRateClientTestsBase
{
    private readonly IDirectExchangeRatesClient _client;

    public DirectExchangeRateTests(KaikoStreamFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _client = fixture.Services.GetRequiredService<IDirectExchangeRatesClient>();
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
            void OnNext(ExchangeRateResponse response)
            {
                AssertResponse(response, symbol, currency);
                replies++;
            }

            void OnError(Exception x)
            {
                Output.WriteLine(x.Message);
            }

            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);
            await _client
                .Observe(request, cancellation.Token)
                .Do(OnNext, OnError)
                .LastOrDefaultAsync();
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().Be(StatusCode.Cancelled);
        }

        replies.Should().BeGreaterThan(0);
    }
}