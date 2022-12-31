using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

public interface IMarketDataClient { }

public abstract class MarketDataClient : IMarketDataClient
{
    protected readonly ICredentialsProvider CredentialProvider;
    protected Uri BaseUrl { get; }

    protected MarketDataClient(ClientConfigurator configurator)
    {
        CredentialProvider = configurator.GetCredentialProvider(GetType());
        BaseUrl = configurator.ApiConfiguration.BaseUrl;
    }
}
