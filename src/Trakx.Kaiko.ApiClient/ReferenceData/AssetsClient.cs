using System.Text;

namespace Trakx.Kaiko.ApiClient;

// this is only here to stop Codacy from flagging issue:
// 'partial' is gratuitous in this context.
internal partial class AssetsClient { }

internal partial class AssetsClient
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
        PrepareRequestBase(client, request, urlBuilder);
    }
}

