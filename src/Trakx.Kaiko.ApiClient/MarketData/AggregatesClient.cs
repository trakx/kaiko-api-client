using System.Text;

namespace Trakx.Kaiko.ApiClient;

internal partial class AggregatesClient
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
        PrepareRequestBase(client, request, urlBuilder);
    }
}

