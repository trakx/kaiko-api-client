using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Kaiko.ApiClient;

public static partial class AddKaikoClientExtensions
{
    public static IServiceCollection AddKaikoClient(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(KaikoApiConfiguration)).Get<KaikoApiConfiguration>();
        services.AddKaikoClient(config);
        return services;
    }

    public static IServiceCollection AddKaikoClient(this IServiceCollection services, KaikoApiConfiguration apiConfiguration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IKaikoCredentialsProvider, ApiKeyCredentialsProvider>();
        services.AddSingleton(apiConfiguration);
        services.AddSingleton<ClientConfigurator>();
        AddClients(services);
        return services;
    }
}