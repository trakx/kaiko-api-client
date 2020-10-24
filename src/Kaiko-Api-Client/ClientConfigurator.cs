using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kaiko_Api_Client
{
    internal class ClientConfigurator
    {
        private readonly IServiceProvider _provider;

        public ClientConfigurator(IServiceProvider provider)
        {
            _provider = provider;
            Configuration = provider.GetService<IOptions<KaikoConfiguration>>().Value;
        }

        public KaikoConfiguration Configuration { get; }
    }
}