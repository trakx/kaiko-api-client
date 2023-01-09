using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

public class ClientConfigurator
{
    public KaikoApiConfiguration ApiConfiguration { get; }
    private readonly IKaikoApiCredentialsProvider _credentialsProvider;

    public ClientConfigurator(KaikoApiConfiguration apiConfiguration,
        IKaikoApiCredentialsProvider credentialsProvider)
    {
        ApiConfiguration = apiConfiguration;
        _credentialsProvider = credentialsProvider;
    }

    public ICredentialsProvider GetCredentialProvider(Type clientType)
    {
        if (clientType.Name == nameof(ReferenceDataClientBase))
        {
            return new NoCredentialsProvider();
        }

        return _credentialsProvider;
    }
}
