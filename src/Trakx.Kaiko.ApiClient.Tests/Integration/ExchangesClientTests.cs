using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Trakx.Kaiko.ApiClient.Tests.Integration
{
    public class ExchangesClientTests : KaikoClientTestsBase
    {
        private readonly IExchangesClient _exchangesClient;

        public ExchangesClientTests()
        {

            _exchangesClient = ServiceProvider.GetRequiredService<IExchangesClient>();
        }

        [Fact]
        public async Task GetAllExchanges_should_return_all_available_exchanges()
        {
            var exchanges = (await _exchangesClient.GetAllExchangesAsync()).Result;

            exchanges.Data.Count.Should().BeGreaterThan(50);
        }
    }
}
