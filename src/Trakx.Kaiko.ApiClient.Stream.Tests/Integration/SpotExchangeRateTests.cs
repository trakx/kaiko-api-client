using System.Runtime.InteropServices;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Stream.Tests.Integration
{
    public class SpotExchangeRateTests : KaikoStreamTestsBase
    {
        public SpotExchangeRateTests(KaikoStreamFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Theory]
        [InlineData("btc")]
        [InlineData("eth", "eur")]
        public async Task Stream_should_return_price_from_exchanges(string symbol, string? currency = "usd")
        {
            var seconds = 5;
            var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

            var replies = 0;
            var validPrices = 0;
            try
            {
                var stream = StreamClient.StreamSpotExchangeRates(symbol, currency, cancellation.Token);
                await foreach (var response in stream)
                {
                    replies++;
                    
                    response.Should().NotBeNull();
                    
                    if (response.Price > 0m) validPrices++;

                    response.Code.Should().Be($"{symbol}-{currency}");
                    
                    Output.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff}:{1}", response.Timestamp, response.Price);
                }
            }
            catch (RpcException x)
            {
                x.StatusCode.Should().Be(StatusCode.Cancelled);
            }

            replies.Should().BeGreaterThan(0);
        }
    }
}