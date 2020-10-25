using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;

namespace Trakx.Kaiko.ApiClient.Tests
{
    public class KaikoClientTests
    {
        private readonly IInstrumentsClient _instrumentsClient;
        private readonly IExchangesClient _exchangesClient;
        private readonly IMarketDataClient _marketDataClient;
        public KaikoClientTests()
        {
            var configuration = new KaikoConfiguration
            {
                ApiKey = ""
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddKaikoClient(configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _instrumentsClient = serviceProvider.GetRequiredService<IInstrumentsClient>();
            _exchangesClient = serviceProvider.GetRequiredService<IExchangesClient>();
            _marketDataClient = serviceProvider.GetRequiredService<IMarketDataClient>();
        }

        [Fact]
        public async Task GetAllExchanges_should_return_all_available_exchanges()
        {
            var exchanges = (await _exchangesClient.GetAllExchangesAsync()).Result;

            exchanges.Data.Count.Should().BeGreaterThan(50);
        }

        [Fact(Skip = "Too long test : 24 seconds but work")]
        //[Fact]
        public async Task GetAllAssets_should_return_all_available_exchanges()
        {
            var assets = (await _instrumentsClient.GetAllInstrumentsAsync()).Result;

            assets.Data.Count.Should().BeGreaterThan(300);
        }

        [Fact(Skip = "Should have apiKey but work")]
        public async Task GetChosenPriceForCurrency_should_return_price()
        {
            var assets = (await _marketDataClient.GetRecentVwapAsync(Commodity.Trades, Data_version.V1, "bcex", "spot", "btc-usdt")).Result;

            assets.Data[0].Price.Should().BeGreaterThan(5000);
        }
    }
}
