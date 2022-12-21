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
        var aws = ConfigurationHelper.GetConfigurationFromAws<KaikoStreamConfiguration>();
        var env = ConfigurationHelper.GetConfigurationFromEnv<KaikoStreamConfiguration>();
        var json = GetConfigurationFromAppSettings();
        return BuildConfiguration(aws, env, json);
    }

    private static KaikoStreamConfiguration BuildConfiguration(
        KaikoStreamConfiguration? aws,
        KaikoStreamConfiguration? env,
        KaikoStreamConfiguration? json)
    {
        return new KaikoStreamConfiguration
        {
            ApiKey = aws?.ApiKey ?? env?.ApiKey ?? json?.ApiKey,
            ChannelUrl = aws?.ChannelUrl ?? env?.ChannelUrl ?? json?.ChannelUrl,
        };
    }

    private static KaikoStreamConfiguration GetConfigurationFromAppSettings()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var result = config.GetRequiredSection("KaikoStreamConfiguration").Get<KaikoStreamConfiguration>();
        return result;
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
