using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Trakx.Kaiko.ApiClient
{
    internal class ClientConfigurator
    {
        public ClientConfigurator(IServiceProvider provider)
        {
            ApiConfiguration = provider.GetService<IOptions<KaikoApiConfiguration>>().Value;
        }

        public KaikoApiConfiguration ApiConfiguration { get; }
    }
}