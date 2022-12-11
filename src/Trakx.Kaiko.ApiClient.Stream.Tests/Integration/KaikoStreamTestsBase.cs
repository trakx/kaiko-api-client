using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Stream.Tests.Integration;

[Collection(nameof(ApiTestCollection))]
public class KaikoStreamTestsBase
{
    protected readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected readonly IKaikoStreamClient StreamClient;

    public KaikoStreamTestsBase(KaikoStreamFixture fixture, ITestOutputHelper output)
    {
        var services = fixture.ServiceProvider;

        Output = output;
        Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();
        StreamClient = services.GetRequiredService<IKaikoStreamClient>();
    }
}


[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<KaikoStreamFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class KaikoStreamFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public KaikoStreamFixture()
    {
        var configuration = ConfigurationHelper.GetConfigurationFromEnv<KaikoStreamConfiguration>();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddKaikoStream(configuration);
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
