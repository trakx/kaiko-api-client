using Microsoft.Extensions.Configuration;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class KaikoStreamFixture : IDisposable
{
    public ServiceProvider Services { get; }
    public KaikoStreamConfiguration Config { get; }

    public KaikoStreamFixture()
    {
        Config = BuildConfiguration();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddKaikoStream(Config);
        Services = serviceCollection.BuildServiceProvider();
    }

    public static KaikoStreamConfiguration BuildConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var json = config.GetRequiredSection("KaikoStreamConfiguration").Get<KaikoStreamConfiguration>();
        var env = ConfigurationHelper.GetConfigurationFromEnv<KaikoStreamConfiguration>();
        var aws = ConfigurationHelper.GetConfigurationFromAws<KaikoStreamConfiguration>();
        return new KaikoStreamConfiguration
        {
            ApiKey = aws?.ApiKey ?? env?.ApiKey ?? json?.ApiKey,
            ChannelUrl = aws?.ChannelUrl ?? env?.ChannelUrl ?? json?.ChannelUrl,
        };
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
