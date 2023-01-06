using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

internal static class HttpClientRegistration
{
    internal static IServiceCollection ConfigureHttpClient<TClient, // NOSONAR
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services)
        where TClient : class
        where TImplementation : class, TClient
    {
        var name = typeof(TImplementation).FullName;

        services
            .AddHttpClient<TClient, TImplementation>(name)
            .AddDecompression()
            .AddPolicyHandler<TImplementation>();

        return services;
    }

    private static IHttpClientBuilder AddDecompression(this IHttpClientBuilder http)
    {
        return http.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
        });
    }

    private static IHttpClientBuilder AddPolicyHandler<TImplementation>(this IHttpClientBuilder http) // NOSONAR
    {
        var medianFirstRetryDelay = TimeSpan.FromMilliseconds(100);
        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay, retryCount: 10, fastFirst: true);

        return http.AddPolicyHandler((_, _) =>
            Policy<HttpResponseMessage>
            .Handle<ApiException>()
            .Or<HttpRequestException>()
            .OrTransientHttpStatusCode()
            .WaitAndRetryAsync(delay,
                onRetry: (result, timeSpan, retryCount, context) =>
                {
                    var logger = Log.Logger.ForContext<TImplementation>();
                    logger.LogApiFailure(result, timeSpan, retryCount, context);
                })
            .WithPolicyKey(typeof(TImplementation).FullName)
        );
    }
}
