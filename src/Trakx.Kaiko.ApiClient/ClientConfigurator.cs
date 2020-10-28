using Microsoft.Extensions.Options;

namespace Trakx.Kaiko.ApiClient
{
    internal class ClientConfigurator
    {
        public ClientConfigurator(IOptions<KaikoApiConfiguration> configuration)
        {
            ApiConfiguration = configuration.Value;
        }

        public KaikoApiConfiguration ApiConfiguration { get; }
    }
}