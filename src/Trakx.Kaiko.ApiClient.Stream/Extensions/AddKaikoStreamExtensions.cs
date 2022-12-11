using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Trakx.Kaiko.ApiClient.Stream;

public static partial class AddKaikoStreamExtensions
{
    public static IServiceCollection AddKaikoStream(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(KaikoStreamConfiguration)).Get<KaikoStreamConfiguration>();
        services.AddKaikoStream(config);
        return services;
    }

    public static IServiceCollection AddKaikoStream(this IServiceCollection services, KaikoStreamConfiguration config)
    {
        services.AddSingleton(config);
        services.AddGrpcClients(config);
        services.AddSingleton<IKaikoStreamClient, KaikoStreamClient>();
        return services;
    }
}