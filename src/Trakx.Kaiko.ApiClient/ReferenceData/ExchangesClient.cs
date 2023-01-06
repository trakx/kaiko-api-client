namespace Trakx.Kaiko.ApiClient;

internal partial class ExchangesClient
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
    {
        PrepareRequestBase(request, urlBuilder);
    }
}

