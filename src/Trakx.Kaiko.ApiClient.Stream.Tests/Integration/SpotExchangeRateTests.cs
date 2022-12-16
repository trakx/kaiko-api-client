namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class SpotExchangeRateTests : KaikoStreamTestsBase
{
    private readonly ISpotExchangeRatesClient _client;

    public SpotExchangeRateTests(KaikoStreamFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _client = fixture.Services.GetRequiredService<ISpotExchangeRatesClient>();
    }

    [Theory]
    [InlineData("btc", "usd")]
    [InlineData("eth", "eur")]
    public async Task Stream_should_return_prices(string symbol, string currency)
    {
        var seconds = 3;
        var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

        var replies = 0;
        var validPrices = 0;
        try
        {
            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);

            await foreach (var response in _client.StreamAsync(request, cancellation.Token))
            {
                replies++;

                response.Should().NotBeNull();

                if (response.Price > 0m) validPrices++;

                response.Symbol.Should().Be(symbol);
                response.Currency.Should().Be(currency);

                Output.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff}:{1}", response.Timestamp, response.Price);
            }
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().Be(StatusCode.Cancelled);
        }

        replies.Should().BeGreaterThan(0);
    }
}