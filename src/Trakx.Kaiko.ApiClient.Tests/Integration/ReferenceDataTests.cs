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

    [Fact]
    public async Task InstrumentsClient_should_return_list_of_instruments()
    {
        var client = ServiceProvider.GetRequiredService<IInstrumentsClient>();
        var response = await client.GetAllInstrumentsAsync();
        response.Should().NotBeNull();
        response.Result.Should().NotBeNull();
        response.Result.Data.Should().NotBeNull();
        response.Result.Data.Should().HaveCountGreaterThan(0);
    }

    [InlineData(false)]
    [Theory]
    public async Task Download_legacy_symbols(bool download)
    {
        if (!download)
        {
            download.Should().BeFalse();
            return;
        }

        var client = ServiceProvider.GetRequiredService<IInstrumentsClient>();
        var response = await client.GetAllInstrumentsAsync();

        using var writer = new StreamWriter("kaiko-legacy-symbols.csv");

        WriteLineItems(writer, "kaiko_legacy_symbol", "base_asset", "quote_asset");

        var currencies = new[] { "usd", "usdc", "eur" };

        var data = response.Result.Data
            .Where(p => p.Class == "spot")
            .Where(p => !string.IsNullOrWhiteSpace(p.Kaiko_legacy_symbol))
            .Where(p => !p.Kaiko_legacy_symbol.StartsWith("0x"))
            .Where(p => currencies.Contains(p.Quote_asset))
            .DistinctBy(p => p.Kaiko_legacy_symbol)
            .OrderBy(p => p.Kaiko_legacy_symbol);

        foreach (var d in data)
        {
            WriteLineItems(writer, d.Kaiko_legacy_symbol, d.Base_asset, d.Quote_asset);
        }

        writer.Close();

        download.Should().BeTrue();
    }

    private static void WriteLineItems(StreamWriter writer, params string[] items)
    {
        var line = string.Join(',', items);
        writer.WriteLine(line);
    }
}
