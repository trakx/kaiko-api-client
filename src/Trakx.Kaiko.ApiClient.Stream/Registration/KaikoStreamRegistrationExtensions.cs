﻿using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Configure Kaiko Stream services from configuration.
/// </summary>
public static partial class KaikoStreamRegistrationExtensions
{
    /// <inheritdoc/>
    public static IServiceCollection AddKaikoStream(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(KaikoStreamConfiguration)).Get<KaikoStreamConfiguration>();
        services.AddKaikoStream(config);
        return services;
    }

    /// <inheritdoc/>
    public static IServiceCollection AddKaikoStream(this IServiceCollection services, KaikoStreamConfiguration config)
    {
        services.AddSingleton(config);
        services.AddGrpcClients(config);
        services.AddSingleton<IDirectExchangeRatesClient, DirectExchangeRatesClient>();
        services.AddSingleton<ISpotExchangeRatesClient, SpotExchangeRatesClient>();
        return services;
    }

    private static ChannelCredentials CreateCredentials(string apiKey)
    {
        var interceptor = CallCredentials.FromInterceptor((_, metadata) =>
        {
            metadata.Add("Authorization", $"Bearer {apiKey}");
            return Task.CompletedTask;
        });

        var ssl = new SslCredentials();
        var credentials = ChannelCredentials.Create(ssl, interceptor);
        return credentials;
    }

    private static ServiceConfig ConfigService(RetryPolicy retryPolicy)
    {
        return new ServiceConfig
        {
            MethodConfigs =
            {
                new MethodConfig
                {
                    Names = { MethodName.Default },
                    //RetryPolicy = retryPolicy
                }
            }
        };
    }
}