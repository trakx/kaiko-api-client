using Serilog;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

[Collection(nameof(ApiTestCollection))]
public class ExchangeRateClientTestsBase
{
    protected readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected readonly ServiceProvider Services;

    public ExchangeRateClientTestsBase(KaikoStreamFixture fixture, ITestOutputHelper output)
    {
        Output = output;
        Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();
        Services = fixture.Services;
    }

    protected void AssertResponse(ExchangeRateResponse response, string expectedSymbol, string expectedCurrency)
    {
        response.Should().NotBeNull();
        response.Symbol.Should().Be(expectedSymbol);
        response.Currency.Should().Be(expectedCurrency);
        Output.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff}:{1}", response.Timestamp, response.Price);
    }
}


[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<KaikoStreamFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
