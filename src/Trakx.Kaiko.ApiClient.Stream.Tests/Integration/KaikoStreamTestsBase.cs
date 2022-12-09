using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Stream.Tests.Integration;

[Collection(nameof(ApiTestCollection))]
public class KaikoClientTestsBase
{
    protected readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected readonly IServiceProvider ServiceProvider;

    public KaikoClientTestsBase(KaikoApiFixture apiFixture, ITestOutputHelper output)
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

        var configuration = ConfigurationHelper.GetConfigurationFromAws<KaikoApiConfiguration>()
            with
        {
            BaseUrl = "https://api.Kaiko.io"
        };

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddKaikoClient(configuration);
        ServiceProvider = serviceCollection.BuildServiceProvider();
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
