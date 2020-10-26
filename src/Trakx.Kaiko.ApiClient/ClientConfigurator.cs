using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Trakx.Kaiko.ApiClient
{
    internal class ClientConfigurator
    {
        public ClientConfigurator(IServiceProvider provider)
        {
            Configuration = provider.GetService<IOptions<KaikoConfiguration>>().Value;
        }

        public KaikoConfiguration Configuration { get; }
    }
}