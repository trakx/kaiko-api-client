using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Grpc.Net.ClientFactory;
using KaikoSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Configure Kaiko Stream services from configuration.
/// </summary>
public static partial class KaikoStreamRegistrationExtensions
{
    /// <inheritdoc/>
    private static void AddGrpcClients(this IServiceCollection services, KaikoStreamConfiguration config)
    {
        void add<T>() where T : class
        {
            services.AddGrpcClient<T>(options =>
            {
                options.Address = new Uri(config.ChannelUrl);
                options.ChannelOptionsActions.Add(o => o.Credentials = CreateCredentials(config.ApiKey));
                options.ChannelOptionsActions.Add(o => o.ServiceConfig = ConfigService(config.RetryPolicy));
            });
        }

        // A T4 template would work great here, extracting all *Client classes from KaikoSdk.dll.
        // Unfortunately, as of 2022-12-15, we can't use .NET 6 assemblies in T4 templates
        // https://stackoverflow.com/questions/60153842/using-net-core-assemblies-in-t4-templates

        add<StreamAggregatesDirectExchangeRateServiceV1.StreamAggregatesDirectExchangeRateServiceV1Client>();
        add<StreamAggregatesSpotExchangeRateServiceV1.StreamAggregatesSpotExchangeRateServiceV1Client>();
        
        //add<StreamAggregatedPriceServiceV1.StreamAggregatedPriceServiceV1Client>();
        //add<StreamAggregatesOHLCVServiceV1.StreamAggregatesOHLCVServiceV1Client>();
        //add<StreamAggregatesVWAPServiceV1.StreamAggregatesVWAPServiceV1Client>();

        //add<StreamIndexServiceV1.StreamIndexServiceV1Client>();
        //add<StreamTradesServiceV1.StreamTradesServiceV1Client>();
        //add<StreamMarketUpdateServiceV1.StreamMarketUpdateServiceV1Client>();
    }
}