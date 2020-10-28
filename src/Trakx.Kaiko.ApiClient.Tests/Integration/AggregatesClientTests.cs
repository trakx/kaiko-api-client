using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Trakx.Kaiko.ApiClient.Tests.Integration
{
    public class AggregatesClientTests : KaikoClientTestsBase
    {
        private readonly IAggregatesClient _aggregatesClient;
        public AggregatesClientTests()
        {
            _aggregatesClient = ServiceProvider.GetRequiredService<IAggregatesClient>();
        }

        [Fact]
        public async Task GetRecentVwapAsync_should_return_results()
        {
            var assets = (await _aggregatesClient.GetRecentVwapAsync(Commodity.Trades, DataVersion.V1, "bcex", "spot", "btc-usdt")).Result;

            assets.Data[0].Price.Should().BeGreaterThan(5000);
        }

        [Fact]
        public async Task GetDirectExchangeRate_should_return_exchange_rate()
        {
            var exchangeRate = await _aggregatesClient.GetDirectExchangeRateAsync(
                Commodity.Trades, DataVersion.Latest, "btc",
                "usdc",
                _aggregatesClient.Top12ExchangeIdsAsCsv,
                start_time: DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(5)),
                interval: "5m",
                sort: SortOrder.Desc,
                page_size: 10);

            exchangeRate.Result.Data.First().Price.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetExchangeRate_should_return_exchange_rate()
        {
            var exchangeRate = await _aggregatesClient.GetExchangeRateAsync(
                "btc",
                "usd",
                "bnce,cbse",
                start_time: DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(5)),
                interval: "5m",
                sort: SortOrder.Desc,
                page_size: 10);

            exchangeRate.Result.Data.First().Price.Should().BeGreaterThan(0);
        }
    }
}
