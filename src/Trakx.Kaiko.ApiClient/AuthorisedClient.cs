using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient
{
    internal abstract class AuthorisedClient : FavouriteExchangesClient
    {
        private string ApiKey => ApiConfiguration!.ApiKey;

        protected AuthorisedClient(ClientConfigurator clientConfigurator) : base(clientConfigurator)
        {
        }

        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();
            msg.Headers.Add("Accept", "application/json");
            msg.Headers.Add("X-Api-Key", ApiKey);
            return Task.FromResult(msg);
        }
    }
}