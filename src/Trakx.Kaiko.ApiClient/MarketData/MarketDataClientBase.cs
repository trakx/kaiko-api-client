using System.Text;
using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

internal abstract class MarketDataClientBase : ClientBase
{
    protected MarketDataClientBase(ClientConfigurator configurator)
        : base(configurator, configurator.ApiConfiguration.MarketDataBaseUrl)
    {
    }
}

