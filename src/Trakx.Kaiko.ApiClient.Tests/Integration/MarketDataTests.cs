using Newtonsoft.Json;
using Trakx.Common.ApiClient;

namespace Trakx.Kaiko.ApiClient.Tests;

public class MarketDataTests : IntegrationTestsBase
{
    private const int PageSize = 3;
    private const string Exchange = "cbse";
    private const string InstrumentClass = "spot";
    private const string TradePair = "btc-usd";

    private readonly DateTime _endTime;
    private readonly DateTime _startTime;

    public MarketDataTests(KaikoApiFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        _endTime = DateTime.UtcNow.Date;
        _startTime = _endTime.AddDays(-7);
    }

    [InlineData(EnabledServices.AggregateOhlcv)]
    [Theory]
    public async Task AggregatesClient_GetAggregateOhlcvAsync_should_return_data(bool serviceEnabled)
    {
        if (!serviceEnabled) return;

        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();

        await Assert_response_and_data(async ()
            => await client.GetAggregateOhlcvAsync(
                Commodity.Trades, DataVersion.V1,
                Exchange, InstrumentClass, TradePair,
                Interval._1d, start_time: _startTime, end_time: _endTime,
                page_size: PageSize, sort: SortOrder.Desc)

            , content => content.Data);
    }

    [InlineData(EnabledServices.AggregateVwap)]
    [Theory]
    public async Task AggregatesClient_GetAggregateVwapAsync_should_return_data(bool serviceEnabled)
    {
        if (!serviceEnabled) return;

        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();

        await Assert_response_and_data(
            async () => await client.GetAggregateVwapAsync(
                Commodity.Trades, DataVersion.V1,
                Exchange, InstrumentClass, TradePair,
                Interval._1d, start_time: _startTime, end_time: _endTime,
                page_size: PageSize, sort: SortOrder.Desc)

            , content => content.Data);
    }

    [InlineData(EnabledServices.AggregateCountOhlcvVwap)]
    [Theory]
    public async Task AggregatesClient_GetAggregateCountOhlcvVwapAsyncAsync_should_return_data(bool serviceEnabled)
    {
        if (!serviceEnabled) return;

        var client = ServiceProvider.GetRequiredService<IAggregatesClient>();

        await Assert_response_and_data(
            async () => await client.GetAggregateCountOhlcvVwapAsync(
                Commodity.Trades, DataVersion.V1,
                Exchange, InstrumentClass, TradePair,
                Interval._1d, start_time: _startTime, end_time: _endTime,
                page_size: PageSize, sort: SortOrder.Desc)

            , content => content.Data);
    }

    [InlineData(EnabledServices.Trades)]
    [Theory]
    public async Task TradesClient_should_return_data(bool serviceEnabled)
    {
        if (!serviceEnabled) return;

        var startTime = _endTime.AddMinutes(-5);

        var client = ServiceProvider.GetRequiredService<ITradesClient>();

        await Assert_response_and_data(
            async () => await client.GetTradesAsync(
                Commodity.Trades, DataVersion.V1,
                Exchange, InstrumentClass, TradePair,
                start_time: startTime, end_time: _endTime,
                page_size: PageSize, sort: SortOrder.Desc)

            , content => content.Data);
    }

    private async Task Assert_response_and_data<T, D>(Func<Task<Response<T>>> action, Func<T, List<D>> getData)
        where T : ApiResponse
    {
        var response = await action();

        response.Should().NotBeNull();
        response.Content.Should().NotBeNull();

        var data = getData(response.Content);
        data.Should().NotBeNull();
        data.Should().HaveCount(PageSize);

        var j = JsonConvert.SerializeObject(data, Formatting.Indented);
        Output.WriteLine(j);
    }
}
