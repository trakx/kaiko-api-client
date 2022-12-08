using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
namespace Trakx.Kaiko.ApiClient.Tests.Integration;

public class TradabilityCheckerTests : KaikoClientTestsBase
{
    private readonly IMarketDataClient _marketDataClient;

    public TradabilityCheckerTests(KaikoApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _marketDataClient = ServiceProvider.GetRequiredService<IMarketDataClient>();
    }

    [Fact]
    public async Task GetTradableAsync_should_return_nothing_for_untradable_symbols()
    {
        var tradabilityChecker = new TradabilityChecker(_marketDataClient);
        var items = await tradabilityChecker.GetTradableAsync(new[] { "bla-bla-bla" });
        items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTradableAsync_should_return_success_for_known_symbols()
    {
        var tradabilityChecker = new TradabilityChecker(_marketDataClient);
        var wideRenownSymbols = new[] { "btc", "eth", "gbp" };
        var items = await tradabilityChecker.GetTradableAsync(wideRenownSymbols);
        items.Should().BeEquivalentTo(wideRenownSymbols);
    }
}
