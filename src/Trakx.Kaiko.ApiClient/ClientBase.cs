using System.Text;
using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

internal abstract class ClientBase
{
    protected readonly ICredentialsProvider CredentialProvider;
    protected Uri BaseUrl { get; }

    protected ClientBase(ClientConfigurator configurator, Uri baseUrl)
    {
        CredentialProvider = configurator.GetCredentialProvider(GetType());
        BaseUrl = baseUrl;
    }

    protected void PrepareRequestBase(HttpRequestMessage request, StringBuilder urlBuilder)
    {
        CredentialProvider.AddCredentials(request);

        var urlStart = BaseUrl.OriginalString.TrimEnd('/') + "/";
        urlBuilder.Insert(0, urlStart);
    }
}

