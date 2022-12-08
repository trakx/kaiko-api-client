using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Kaiko.ApiClient.Utils;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Kaiko.ApiClient;

public static partial class AddKaikoClientExtensions
{
    public static IServiceCollection AddKaikoClient(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(KaikoApiConfiguration)).Get<KaikoApiConfiguration>();
        serviceCollection.AddKaikoClient(config);
        return serviceCollection;
    }

    public static IServiceCollection AddKaikoClient(
        this IServiceCollection serviceCollection, KaikoApiConfiguration apiConfiguration)
    {
        serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        serviceCollection.AddSingleton<IKaikoCredentialsProvider, ApiKeyCredentialsProvider>();
        serviceCollection.AddSingleton(apiConfiguration);
        serviceCollection.AddSingleton<ClientConfigurator>();
        AddClients(serviceCollection);
        return serviceCollection;
    }
}