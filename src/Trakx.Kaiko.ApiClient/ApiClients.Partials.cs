
namespace Trakx.Kaiko.ApiClient;

internal partial class AccountsClient
{
    partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
    {
        CredentialProvider.AddCredentials(request);
    }
}