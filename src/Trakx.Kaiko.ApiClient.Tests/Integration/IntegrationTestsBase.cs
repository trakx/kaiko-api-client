using System;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Tests;

[Collection(nameof(ApiTestCollection))]
public class IntegrationTestsBase
{
    protected readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected readonly IServiceProvider ServiceProvider;

    public IntegrationTestsBase(KaikoApiFixture apiFixture, ITestOutputHelper output)
    {
        Output = output;
        ServiceProvider = apiFixture.ServiceProvider;
        Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();
    }
}


[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<KaikoApiFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class KaikoApiFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public KaikoApiFixture()
    {
        var config = BuildConfiguration();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddKaikoClient(config);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public static KaikoApiConfiguration BuildConfiguration()
    {
        var aws = ConfigurationHelper.GetConfigurationFromAws<KaikoApiConfiguration>();
        var json = GetConfigurationFromAppSettings();
        return new KaikoApiConfiguration
        {
            ApiKey = aws?.ApiKey,
            MarketDataBaseUrl = json?.MarketDataBaseUrl,
            ReferenceDataBaseUrl = json?.ReferenceDataBaseUrl,
        };
    }

    private static KaikoApiConfiguration GetConfigurationFromAppSettings()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var result = config.GetRequiredSection("KaikoApiConfiguration").Get<KaikoApiConfiguration>();
        return result;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        ServiceProvider.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
