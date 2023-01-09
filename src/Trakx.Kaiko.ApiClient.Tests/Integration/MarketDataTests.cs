using System.Reactive;
using Newtonsoft.Json;
using Serilog;
using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient.Tests;

public class MarketDataTests : IntegrationTestsBase
{
    public MarketDataTests(KaikoApiFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public async Task AggregatesClient_GetAggregateOhlcvAsync_should_return_data()
    {
        var expectedResults = 3;
        var endTime = DateTime.UtcNow.Date;
        var startTime = endTime.AddDays(-expectedResults);

        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();
        var response = await client.GetAggregateOhlcvAsync(
            Commodity.Trades, DataVersion.Latest,
            "cbse", "spot", "btc-usd", Interval._1d,
            end_time: endTime, start_time: startTime, sort: SortOrder.Desc);

        response.Should().NotBeNull();
        response.Result.Should().NotBeNull();
        Response_should_have_data(response.Result.Data, expectedResults);
    }

    [Fact]
    public async Task AggregatesClient_GetAggregateVwapAsync_should_return_data()
    {
        var expectedResults = 3;
        var endTime = DateTime.UtcNow.Date;
        var startTime = endTime.AddDays(-expectedResults);

        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();
        var response = await client.GetAggregateVwapAsync(
            Commodity.Trades, DataVersion.Latest,
            "cbse", "spot", "btc-usd", Interval._1d,
            end_time: endTime, start_time: startTime, sort: SortOrder.Desc);
            
        response.Should().NotBeNull();
        response.Result.Should().NotBeNull();
        Response_should_have_data(response.Result.Data, expectedResults);
    }

    private void Response_should_have_data<TData>(List<TData> data, int expectedResults)
    {
        data.Should().NotBeNull();
        data.Should().HaveCount(expectedResults);

        var j = JsonConvert.SerializeObject(data, Formatting.Indented);
        Output.WriteLine(j);
    }
}
