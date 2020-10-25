using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Trakx.Kaiko.ApiClient
{
    public static class AddKaikoClientExtension
    {
        public static IServiceCollection AddKaikoClient(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<KaikoConfiguration>(
                configuration.GetSection(nameof(KaikoConfiguration)));
            AddCommonDependencies(services);

            return services;
        }

        public static IServiceCollection AddKaikoClient(
            this IServiceCollection services, KaikoConfiguration configuration)
        {
            var options = Options.Create(configuration);
            services.AddSingleton(options);
            AddCommonDependencies(services);

            return services;
        }

        private static void AddCommonDependencies(IServiceCollection services)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(100), retryCount: 10, fastFirst: true);

            services.AddSingleton(s => new ClientConfigurator(s));

            services.AddHttpClient<IAssetsClient, AssetsClient>()
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>.Handle<ApiException>()
                        .Or<HttpRequestException>()
                        .OrTransientHttpStatusCode()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        .WaitAndRetryAsync(delay,
                            (result, timeSpan, retryCount, context) =>
                            {
                                var logger = s.GetService<ILogger<IAssetsClient>>();
                                LogFailure(logger, result, timeSpan, retryCount, context);
                            })
                        .WithPolicyKey("AssetClient"));
            
            services.AddHttpClient<IExchangesClient, ExchangesClient>()
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>.Handle<ApiException>()
                        .Or<HttpRequestException>()
                        .OrTransientHttpStatusCode()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        .WaitAndRetryAsync(delay,
                            (result, timeSpan, retryCount, context) =>
                            {
                                var logger = s.GetService<ILogger<IExchangesClient>>();
                                LogFailure(logger, result, timeSpan, retryCount, context);
                            })
                        .WithPolicyKey("ExchangesClient"));

            services.AddHttpClient<IMarketDataClient, MarketDataClient>()
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>.Handle<ApiException>()
                        .Or<HttpRequestException>()
                        .OrTransientHttpStatusCode()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        .WaitAndRetryAsync(delay,
                            (result, timeSpan, retryCount, context) =>
                            {
                                var logger = s.GetService<ILogger<IMarketDataClient>>();
                                LogFailure(logger, result, timeSpan, retryCount, context);
                            })
                        .WithPolicyKey("MarketDataClient"));

        }
        private static void LogFailure<T>(ILogger<T> logger, DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, Context context)
        {
            if (result.Exception != null)
            {
                logger.LogWarning(result.Exception, "An exception occurred on retry {RetryAttempt} for {PolicyKey}. Retrying in {SleepDuration}ms.",
                    retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            }
            else
            {
                logger.LogWarning("A non success code {StatusCode} with reason {Reason} and content {Content} was received on retry {RetryAttempt} for {PolicyKey}. Retrying in {SleepDuration}ms.",
                    (int)result.Result.StatusCode, result.Result.ReasonPhrase,
                    result.Result.Content?.ReadAsStringAsync().GetAwaiter().GetResult(),
                    retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            }
        }
    }
}