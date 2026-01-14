using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trakx.Common.ApiClient.Exceptions;
using Trakx.Common.Configuration;
using Trakx.Common.DateAndTime;

namespace Trakx.Kaiko.ApiClient;

public static class KaikoClientRegistration
{
    public static IServiceCollection AddKaikoClient(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetConfiguration<KaikoApiConfiguration>();
        services.AddKaikoClient(config!);
        return services;
    }

    public static IServiceCollection AddKaikoClient(this IServiceCollection services, KaikoApiConfiguration apiConfiguration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IKaikoApiCredentialsProvider, ApiKeyCredentialsProvider>();
        services.AddSingleton(apiConfiguration);
        services.AddSingleton<ClientConfigurator>();
        services.ConfigureApiClients(apiConfiguration);
        return services;
    }

    private static IServiceCollection ConfigureApiClients(this IServiceCollection services, KaikoApiConfiguration apiConfiguration)
    {
        // market data
        services.ConfigureHttpClient<IAggregatesClient, AggregatesClient>(apiConfiguration.MarketDataBaseUrl);
        services.ConfigureHttpClient<ITradesClient, TradesClient>(apiConfiguration.MarketDataBaseUrl);

        // reference data
        services.ConfigureHttpClient<IAssetsClient, AssetsClient>(apiConfiguration.ReferenceDataBaseUrl);
        services.ConfigureHttpClient<IExchangesClient, ExchangesClient>(apiConfiguration.ReferenceDataBaseUrl);
        services.ConfigureHttpClient<IInstrumentsClient, InstrumentsClient>(apiConfiguration.ReferenceDataBaseUrl);

        return services;
    }

    internal static IServiceCollection ConfigureHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        Uri baseUrl)
        where TClient : class
        where TImplementation : class, TClient
    {
        services
            .AddHttpClientForApiClient<TClient, TImplementation>(baseUrl)
            .WithDefaultDelays()
            .WithDefaultPolicy<ApiException>()
            .WithDecompression()
            .Bind()
            .SetCustomLogger();

        return services;
    }
}
