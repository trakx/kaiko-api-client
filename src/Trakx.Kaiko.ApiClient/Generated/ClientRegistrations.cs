




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
            

            services.AddHttpClient<IAggregatesClient, AggregatesClient>("Trakx.Kaiko.ApiClient.AggregatesClient")
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
                    .WithPolicyKey("Trakx.Kaiko.ApiClient.AggregatesClient"));

            

            services.AddHttpClient<IExchangesClient, ExchangesClient>("Trakx.Kaiko.ApiClient.ExchangesClient")
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
                    .WithPolicyKey("Trakx.Kaiko.ApiClient.ExchangesClient"));

            
        }
    }
}
