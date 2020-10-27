using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Trakx.Kaiko.ApiClient.Tests.Integration
{
    public class InstrumentsClientTests : KaikoClientTestsBase
    {
        private readonly IInstrumentsClient _instrumentsClient;

        public InstrumentsClientTests()
        {
            _instrumentsClient = ServiceProvider.GetRequiredService<IInstrumentsClient>();
        }

        [Fact]
        public async Task GetAllAssets_should_return_all_available_exchanges()
        {
            var assets = (await _instrumentsClient.GetAllInstrumentsAsync()).Result;

            assets.Data.Count.Should().BeGreaterThan(300);
        }
    }
}
