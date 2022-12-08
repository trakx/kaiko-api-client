using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

public abstract class AuthorisedClient : FavouriteExchangesClient
{
    protected readonly ICredentialsProvider CredentialProvider;
    protected string BaseUrl { get; }

    protected AuthorisedClient(ClientConfigurator configurator) : base(configurator)
    {
        CredentialProvider = configurator.GetCredentialProvider(GetType());
        BaseUrl = configurator.ApiConfiguration.BaseUrl;
    }
}