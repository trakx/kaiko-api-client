using System.Text;

namespace Trakx.Kaiko.ApiClient;

internal partial class InstrumentsClient
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
        PrepareRequestBase(client, request, urlBuilder);
    }
}

