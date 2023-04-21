using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trakx.Common.DateAndTime;

namespace Trakx.Kaiko.ApiClient;

public static class KaikoClientRegistration
{
    public static IServiceCollection AddKaikoClient(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(KaikoApiConfiguration)).Get<KaikoApiConfiguration>();
        services.AddKaikoClient(config!);
        return services;
    }

    public static IServiceCollection AddKaikoClient(this IServiceCollection services, KaikoApiConfiguration apiConfiguration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IKaikoApiCredentialsProvider, ApiKeyCredentialsProvider>();
        services.AddSingleton(apiConfiguration);
        services.AddSingleton<ClientConfigurator>();
        services.ConfigureApiClients();
        return services;
    }

    private static IServiceCollection ConfigureApiClients(this IServiceCollection services)
    {
        // market data
        services.ConfigureHttpClient<IAggregatesClient, AggregatesClient>();

        // reference data
        services.ConfigureHttpClient<IAssetsClient, AssetsClient>();
        services.ConfigureHttpClient<IExchangesClient, ExchangesClient>();
        services.ConfigureHttpClient<IInstrumentsClient, InstrumentsClient>();

        return services;
    }
}
