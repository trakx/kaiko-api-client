using System.Reactive;
using Newtonsoft.Json;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Tests;

public class MarketDataTests : IntegrationTestsBase
{
    public MarketDataTests(KaikoApiFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public async Task AggregatesClient_should_return_spot_exchange_rate_price()
    {
        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();
        var response = await client.GetAggregateOhlcvAsync(
            Commodity.Trades, DataVersion.Latest,
            "cbse", "spot", "btc-usd", Interval._1s);
            
        response.Should().NotBeNull();
        response.Result.Should().NotBeNull();
        response.Result.Data.Should().NotBeNull();
        response.Result.Data.Should().HaveCountGreaterThan(0);

        var j = JsonConvert.SerializeObject(response.Result.Data, Formatting.Indented);
        Output.WriteLine(j);
    }
}
