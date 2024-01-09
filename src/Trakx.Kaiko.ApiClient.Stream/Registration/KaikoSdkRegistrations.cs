using KaikoSdk;
using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Kaiko.ApiClient.Stream
{
    /// <summary>
    /// Configure Kaiko Stream services from configuration.
    /// </summary>
    public static partial class KaikoStreamRegistrationExtensions
    {
        /// <inheritdoc/>
        internal static void AddGrpcClients(this IServiceCollection services, KaikoStreamConfiguration config)
        {
            void Add<T>() where T : class
            {
                services.AddGrpcClient<T>(options =>
                {
                    options.Address = config.ChannelUrl;
                    options.ChannelOptionsActions.Add(o => o.Credentials = CreateCredentials(config.ApiKey));
                    options.ChannelOptionsActions.Add(o => o.ServiceConfig = ConfigService());
                });
            }

            // A T4 template would work great here, extracting all *Client classes from KaikoSdk.dll.
            // Unfortunately, as of 2022-12-15, we can't use .NET 6 assemblies in T4 templates
            // https://stackoverflow.com/questions/60153842/using-net-core-assemblies-in-t4-templates

            Add<StreamAggregatedPriceServiceV1.StreamAggregatedPriceServiceV1Client>();
            Add<StreamAggregatesOHLCVServiceV1.StreamAggregatesOHLCVServiceV1Client>();
            Add<StreamAggregatesVWAPServiceV1.StreamAggregatesVWAPServiceV1Client>();
            Add<StreamIndexServiceV1.StreamIndexServiceV1Client>();
            Add<StreamMarketUpdateServiceV1.StreamMarketUpdateServiceV1Client>();
            Add<StreamTradesServiceV1.StreamTradesServiceV1Client>();
        }
    }
}

