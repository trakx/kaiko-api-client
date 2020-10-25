using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient
{
    internal abstract class AuthorisedClient
    {
        public KaikoConfiguration Configuration { get; protected set; }

        private string ApiKey => Configuration!.ApiKey;

        public AuthorisedClient(ClientConfigurator clientConfigurator)
        {
            Configuration = clientConfigurator.Configuration;
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