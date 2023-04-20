using Microsoft.Extensions.Configuration;
using Trakx.Common.Testing.Configuration;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class KaikoStreamFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }
    public KaikoStreamConfiguration Config { get; }

    public KaikoStreamFixture()
    {
        Config = BuildConfiguration();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddKaikoStream(Config);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public static KaikoStreamConfiguration BuildConfiguration()
    {
        var aws = AwsConfigurationHelper.GetConfigurationFromAws<KaikoStreamConfiguration>();
        var json = GetConfigurationFromAppSettings();
        return new KaikoStreamConfiguration
        {
            ApiKey = aws?.ApiKey,
            ChannelUrl = json?.ChannelUrl,
        };
    }

    private static KaikoStreamConfiguration GetConfigurationFromAppSettings()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var result = config.GetRequiredSection("KaikoStreamConfiguration").Get<KaikoStreamConfiguration>();
        return result!;
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
