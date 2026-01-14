namespace Trakx.Kaiko.ApiClient;

internal abstract class ClientBase
{
    private readonly ICredentialsProvider _credentialProvider;

    protected ClientBase(ClientConfigurator configurator)
    {
        _credentialProvider = configurator.GetCredentialProvider(GetType());
    }

    protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
    {
        HttpRequestMessage httpRequestMessage = new();
        await _credentialProvider.AddCredentialsAsync(httpRequestMessage);
        return httpRequestMessage;
    }
}