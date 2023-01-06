using System.Text;

namespace Trakx.Kaiko.ApiClient;

// this is only here to stop Codacy from flagging issue:
// 'partial' is gratuitous in this context.
internal partial class AggregatesClient { }

internal partial class AggregatesClient
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
        PrepareRequestBase(client, request, urlBuilder);
    }
}

