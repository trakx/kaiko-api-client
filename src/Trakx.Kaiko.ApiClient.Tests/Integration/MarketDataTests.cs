using Newtonsoft.Json;
using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient.Tests;

public class MarketDataTests : IntegrationTestsBase
{
    public MarketDataTests(KaikoApiFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [InlineData(EnabledServices.AggregateOhlcv)]
    [Theory]
    public async Task AggregatesClient_GetAggregateOhlcvAsync_should_return_data(bool serviceEnabled)
    {
        var expectedResults = 3;
        var endTime = DateTime.UtcNow.Date;
        var startTime = endTime.AddDays(-expectedResults);

        async Task<Response<AggregateOhlcvResponse>> Action(IAggregatesClient client) =>
            await client.GetAggregateOhlcvAsync(
            Commodity.Trades, DataVersion.Latest,
            "cbse", "spot", "btc-usd", Interval._1d,
            end_time: endTime, start_time: startTime, sort: SortOrder.Desc);

        await Response_should_have_data_if_subscription_enabled(Action, serviceEnabled, expectedResults);
    }

    [InlineData(EnabledServices.AggregateVwap)]
    [Theory]
    public async Task AggregatesClient_GetAggregateVwapAsync_should_return_data(bool serviceEnabled)
    {
        var expectedResults = 3;
        var endTime = DateTime.UtcNow.Date;
        var startTime = endTime.AddDays(-expectedResults);

        async Task<Response<AggregateVwapResponse>> Action(IAggregatesClient client) =>
            await client.GetAggregateVwapAsync(
            Commodity.Trades, DataVersion.Latest,
            "cbse", "spot", "btc-usd", Interval._1d,
            end_time: endTime, start_time: startTime, sort: SortOrder.Desc);

        await Response_should_have_data_if_subscription_enabled(Action, serviceEnabled, expectedResults);
    }

    private async Task Response_should_have_data_if_subscription_enabled<T>(
        Func<IAggregatesClient, Task<Response<T>>> action, bool serviceEnabled, int expectedResults)
        where T : ApiResponse
    {
        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();

        var response = await action(client);

        if (!serviceEnabled)
        {
            return;
        }

        response.Should().NotBeNull();
        response.Result.Should().NotBeNull();

        var data = response.Result.Data;
        data.Should().NotBeNull();
        data.Should().HaveCount(expectedResults);

        var j = JsonConvert.SerializeObject(data, Formatting.Indented);
        Output.WriteLine(j);
    }
}
