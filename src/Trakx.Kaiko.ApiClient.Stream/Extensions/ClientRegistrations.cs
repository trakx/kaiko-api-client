using MathNet.Numerics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Contrib.WaitAndRetry;
using Polly;
using Trakx.Utils.DateTimeHelpers;
using Polly.Extensions.Http;
using Serilog;
using Grpc.Net.Client.Configuration;
using Grpc.Core;
using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream;

public static partial class AddKaikoStreamExtensions
{
    private static void AddGrpcClients(this IServiceCollection services, KaikoStreamConfiguration config)
    {
        services.AddGrpcClient<StreamAggregatesSpotExchangeRateServiceV1Client>(options =>
        {
            options.Address = new Uri(config.ChannelUrl);
            options.ChannelOptionsActions.Add(o => o.Credentials = CreateCredentials(config.ApiKey));
            options.ChannelOptionsActions.Add(o => o.ServiceConfig = ConfigService());
        });
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

    private static ServiceConfig ConfigService()
    {
        return new ServiceConfig
        {
            MethodConfigs =
            {
                new MethodConfig
                {
                    Names = { MethodName.Default },
                    RetryPolicy = RetryPolicy()
                }
            }
        };
    }

    private static RetryPolicy RetryPolicy()
    {
        return new RetryPolicy
        {
            MaxAttempts = 5,
            InitialBackoff = TimeSpan.FromSeconds(1),
            MaxBackoff = TimeSpan.FromSeconds(5),
            BackoffMultiplier = 1.5,
            RetryableStatusCodes = { StatusCode.Unavailable }
        };
    }
}