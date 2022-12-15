using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class KaikoStreamFixture : IDisposable
{
    public ServiceProvider Services { get; }

    public KaikoStreamFixture()
    {
        var configuration = ConfigurationHelper.GetConfigurationFromEnv<KaikoStreamConfiguration>();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddKaikoStream(configuration);
        Services = serviceCollection.BuildServiceProvider();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        Services.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
