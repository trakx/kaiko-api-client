using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Kaiko.ApiClient.Tests.Integration
{
    public class KaikoClientTestsBase
    {
        protected ServiceProvider? ServiceProvider;

        public KaikoClientTestsBase()
        {
            var configuration = new KaikoApiConfiguration
            {
                ApiKey = Secrets.KaikoApiKey
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddKaikoClient(configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}