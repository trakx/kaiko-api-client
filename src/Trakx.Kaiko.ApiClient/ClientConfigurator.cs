using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

public class ClientConfigurator
{
    public KaikoApiConfiguration ApiConfiguration { get; }
    private readonly IKaikoCredentialsProvider _credentialsProvider;

    public ClientConfigurator(KaikoApiConfiguration apiConfiguration,
        IKaikoCredentialsProvider credentialsProvider)
    {
        ApiConfiguration = apiConfiguration;
        _credentialsProvider = credentialsProvider;
    }

    public ICredentialsProvider GetCredentialProvider(Type clientType)
    {
        return clientType.Name switch
        {
            nameof(ReferenceDataClient) => new NoCredentialsProvider(),
            _ => _credentialsProvider
        };
    }
}