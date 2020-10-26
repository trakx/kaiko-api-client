using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;


namespace Trakx.Kaiko.ApiClient
{
    public static partial class AddKaikoClientExtension
    {
        private static void AddClients(this IServiceCollection services)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(100), retryCount: 10, fastFirst: true);
                                    
            services.AddHttpClient<IAggregatesClient, AggregatesClient>()
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>.Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<AggregatesClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("AggregatesClient"));

                                
            services.AddHttpClient<IExchangesClient, ExchangesClient>()
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>.Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<ExchangesClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("ExchangesClient"));

                                
            services.AddHttpClient<IInstrumentsClient, InstrumentsClient>()
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>.Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<InstrumentsClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("InstrumentsClient"));

        }
    }
}
