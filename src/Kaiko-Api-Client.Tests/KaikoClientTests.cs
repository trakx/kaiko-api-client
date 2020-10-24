using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;


namespace Kaiko_Api_Client.Tests
{
    public class KaikoClientTests
    {
        private readonly IClient _apiClient;
        public KaikoClientTests()
        {
            var configuration = new KaikoConfiguration()
            {
                ApiKey = ""
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddKaikoClient(configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _apiClient = serviceProvider.GetRequiredService<IClient>();
        }

        public async Task GetAllExchanges_should_return_all_available_exchanges()
        {
            var exchanges = (await _apiClient.GetAllExchangesAsync()).Result;

            exchanges.Data.Count.Should().BeGreaterThan(50);
        }

        [Fact(Skip = "Should have apiKey")]
        public async Task GetAllAssets_should_return_all_available_exchanges()
        {
            var assets = (await _apiClient.GetAllInstrumentsAsync()).Result;

            assets.Data.Count.Should().BeGreaterThan(150);
        }

        [Fact(Skip = "Should have apiKey")]
        public async Task GetChosenPriceForCurrency_should_return_price()
        {
            var assets = (await _apiClient.GetRecentVwapAsync(Commodity.Trades, Data_version.V1, "bcex", "spot", "btc-usdt")).Result;

            assets.Data[0].Price.Should().BeGreaterThan(5000);
        }
    }
}
