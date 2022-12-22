




using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using Trakx.Utils.Apis;


namespace Trakx.Kaiko.ApiClient
{
    public static partial class AddKaikoClientExtensions
    {
        private static void AddClients(this IServiceCollection services)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(100), retryCount: 10, fastFirst: true);
            var currentNamespace = typeof(AddKaikoClientExtensions).Namespace;
            
            var nameOfAggregatesClient = currentNamespace + nameof(AggregatesClient);
            services
                .AddHttpClient<IAggregatesClient, AggregatesClient>(nameOfAggregatesClient)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                        .Handle<ApiException>()
                        .Or<HttpRequestException>()
                        .OrTransientHttpStatusCode()
                        .WaitAndRetryAsync(delay,
                            onRetry: (result, timeSpan, retryCount, context) =>
                            {
                                var logger = Log.Logger.ForContext<AggregatesClient>();
                                logger.LogApiFailure(result, timeSpan, retryCount, context);
                            })
                        .WithPolicyKey(nameOfAggregatesClient));
            
            var nameOfExchangesClient = currentNamespace + nameof(ExchangesClient);
            services
                .AddHttpClient<IExchangesClient, ExchangesClient>(nameOfExchangesClient)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                        .Handle<ApiException>()
                        .Or<HttpRequestException>()
                        .OrTransientHttpStatusCode()
                        .WaitAndRetryAsync(delay,
                            onRetry: (result, timeSpan, retryCount, context) =>
                            {
                                var logger = Log.Logger.ForContext<ExchangesClient>();
                                logger.LogApiFailure(result, timeSpan, retryCount, context);
                            })
                        .WithPolicyKey(nameOfExchangesClient));
            
        }
    }
}
