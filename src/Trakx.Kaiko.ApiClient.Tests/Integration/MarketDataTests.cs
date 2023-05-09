using Newtonsoft.Json;
using Trakx.Common.ApiClient;

namespace Trakx.Kaiko.ApiClient.Tests;

public class MarketDataTests : IntegrationTestsBase
{
    private const int ExpectedResults = 3;
    private readonly DateTime _endTime;
    private readonly DateTime _startTime;

    public MarketDataTests(KaikoApiFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        _endTime = DateTime.UtcNow.Date;
        _startTime = _endTime.AddDays(-ExpectedResults);
    }

    [InlineData(EnabledServices.AggregateOhlcv)]
    [Theory]
    public async Task AggregatesClient_GetAggregateOhlcvAsync_should_return_data(bool serviceEnabled)
    {
        if (!serviceEnabled) return;

        await Assert_response_and_data(async client

            => await client.GetAggregateOhlcvAsync(
                Commodity.Trades, DataVersion.V1, "cbse", "spot", "btc-usd",
                Interval._1d, end_time: _endTime, start_time: _startTime, sort: SortOrder.Desc)

            , content => content.Data);
    }

    [InlineData(EnabledServices.AggregateVwap)]
    [Theory]
    public async Task AggregatesClient_GetAggregateVwapAsync_should_return_data(bool serviceEnabled)
    {
        if (!serviceEnabled) return;

        await Assert_response_and_data(async client

            => await client.GetAggregateVwapAsync(
                Commodity.Trades, DataVersion.V1, "cbse", "spot", "btc-usd",
                Interval._1d, end_time: _endTime, start_time: _startTime, sort: SortOrder.Desc)

            , content => content.Data);
    }

    [InlineData(EnabledServices.AggregateCountOhlcvVwap)]
    [Theory]
    public async Task AggregatesClient_GetAggregateCountOhlcvVwapAsyncAsync_should_return_data(bool serviceEnabled)
    {
        if (!serviceEnabled) return;

        await Assert_response_and_data(async client

            => await client.GetAggregateCountOhlcvVwapAsync(
                Commodity.Trades, DataVersion.V1, "cbse", "spot", "btc-usd",
                Interval._1d, end_time: _endTime, start_time: _startTime, sort: SortOrder.Desc)

            , content => content.Data);
    }

    private async Task Assert_response_and_data<T, D>(
        Func<IAggregatesClient, Task<Response<T>>> action, Func<T, List<D>> getData)
        where T : ApiResponse
    {
        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();

        var response = await action(client);

        response.Should().NotBeNull();
        response.Content.Should().NotBeNull();

        var data = getData(response.Content);
        data.Should().NotBeNull();
        data.Should().HaveCount(ExpectedResults);

        var j = JsonConvert.SerializeObject(data, Formatting.Indented);
        Output.WriteLine(j);
    }
}
