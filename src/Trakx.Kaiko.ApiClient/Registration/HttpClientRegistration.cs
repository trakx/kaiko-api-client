using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;

namespace Trakx.Kaiko.ApiClient;

internal static class HttpClientRegistration
{
    internal static IServiceCollection ConfigureHttpClient<TClient,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services)
        where TClient : class
        where TImplementation : class, TClient
    {
        var name = typeof(TImplementation).FullName;

        services
            .AddHttpClient<TClient, TImplementation>(name!)
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

    private static IHttpClientBuilder AddPolicyHandler<TImplementation>(this IHttpClientBuilder http)
    {
        var medianFirstRetryDelay = TimeSpan.FromMilliseconds(100);
        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay, retryCount: 10, fastFirst: true);

        return http.AddPolicyHandler((_, _) =>
            Policy<HttpResponseMessage>
            .Handle<ApiException>()
            .Or<HttpRequestException>()
            .OrTransientHttpStatusCode()
            .WaitAndRetryAsync(delay,
                onRetry: async (result, timeSpan, retryCount, context) =>
                {
                    var logger = Log.Logger.ForContext<TImplementation>();
                    await logger.LogApiFailure(result, timeSpan, retryCount, context);
                })
            .WithPolicyKey(typeof(TImplementation).FullName)
        );
    }

    private static async Task LogApiFailure(this Serilog.ILogger logger,
        DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, Context context)
    {
        if (result.Exception != null)
        {
            logger.Warning(
                result.Exception,
                "An exception occurred on retry {RetryAttempt} for {PolicyKey} - Retrying in {SleepDuration}ms",
                retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            return;
        }

        var message = result.Result;
        if (message == null) return;

        var content = await message.Content.ReadAsStringAsync();

        logger.Warning(
            "A non success code {StatusCode} with reason {Reason} and content {Content} was received on retry {RetryAttempt} for {PolicyKey} - Retrying in {SleepDuration}ms",
            (int)message.StatusCode, message.ReasonPhrase, content, retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
    }
}
