using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Kaiko.ApiClient.Stream.Tests.Integration
{
    public class KaikoClientStreamTestsBase
    {
        protected ServiceProvider? ServiceProvider;
        protected readonly IOptions<KaikoApiConfiguration> _config;
        protected readonly List<string> _pairs;
        protected ITestOutputHelper _output;

        public KaikoClientStreamTestsBase()
        {
            _config = Options.Create(new KaikoApiConfiguration
            {
                ApiKey = Secrets.KaikoApiKey,
                TargetChannel = Secrets.TargetChannel
            });

            //create list of selected crypto pairs
            _pairs = new List<string>() 
            {
                "btc-usd",
                "eth-usd",
                "ltc-usd",
                "xrp-usd",
                "bch-usd",
                "xmr-btc",
                "doge-usdt",
                "doge-btc",
                "luna-busd",
                "doge-usd"
            };
           
        }
    }
}