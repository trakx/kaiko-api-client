using System.Text;

namespace Trakx.Kaiko.ApiClient;

// this is only here to stop Codacy from flagging issue:
// 'partial' is gratuitous in this context.
internal partial class TradesClient { }

internal partial class TradesClient
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
        PrepareRequestBase(client, request, urlBuilder);
    }
}

