using Serilog;

namespace Trakx.Kaiko.ApiClient.Tests;

public class ReferenceDataTests : IntegrationTestsBase
{
    public ReferenceDataTests(KaikoApiFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public async Task ExchangesClient_should_return_list_of_exchanges()
    {
        var client = ServiceProvider.GetRequiredService<IExchangesClient>();
        var response = await client.GetAllExchangesAsync();
        response.Should().NotBeNull();
        response.Result.Should().NotBeNull();
        response.Result.Data.Should().NotBeNull();
        response.Result.Data.Should().HaveCountGreaterThan(0);

        foreach (var item in response.Result.Data.OrderBy(p => p.Name))
        {
            Output.WriteLine($"[{item.Code}] {item.Name}");
        }
    }

    [Fact]
    public async Task AssetsClient_should_return_list_of_assets()
    {
        var client = ServiceProvider.GetRequiredService<IAssetsClient>();
        var response = await client.GetAllAssetsAsync();
        response.Should().NotBeNull();
        response.Result.Should().NotBeNull();
        response.Result.Data.Should().NotBeNull();
        response.Result.Data.Should().HaveCountGreaterThan(0);

        foreach (var item in response.Result.Data.OrderBy(p => p.Name))
        {
            Output.WriteLine($"[{item.Code}] {item.Name}");
        }
    }
}
