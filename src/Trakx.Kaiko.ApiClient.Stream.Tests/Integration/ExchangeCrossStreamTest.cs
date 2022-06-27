using Xunit;
using FluentAssertions;
using Xunit.Abstractions;

namespace Trakx.Kaiko.ApiClient.Stream.Tests.Integration
{
    public class ExchangeCrossStreamTest : KaikoClientStreamTestsBase
    {        
        private readonly KaikoExchangeCrossStreamHandler _streamClient;        

        public ExchangeCrossStreamTest(ITestOutputHelper output)
        {
            _streamClient = new KaikoExchangeCrossStreamHandler(_config, 
                new StreamRequestParameter("btc-usd", StreamRequestParameterAggregate.OneMinute));

            _output = output;
        }

        /// <summary>
        /// tests the subscribtion of the stream
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task test_incoming_streamAsync() 
        {
            List<IncomingCurrencyPair> receivedPairs = new List<IncomingCurrencyPair>();

            _streamClient.Subscribe((IncomingCurrencyPair obj) =>
            {
                receivedPairs.Add(obj);

                _output.WriteLine($"Observer: Price of {obj.Symbol} is {obj.Price}");
            });

            _streamClient.Start();

            await Task.Delay(5000);

            _streamClient.Stop();

            _streamClient.Dispose();

            receivedPairs.Count.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// tests if stream stops after calling the Stop method
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task test_termination_of_incoming_streamAsync()
        {
            List<IncomingCurrencyPair> receivedPairs = new List<IncomingCurrencyPair>();

            _streamClient.Subscribe((IncomingCurrencyPair obj) =>
            {
                receivedPairs.Add(obj);

                _output.WriteLine($"Observer: Price of {obj.Symbol} is {obj.Price}");
            });

            _streamClient.Start();

            await Task.Delay(1000);            

            _streamClient.Stop();

            _streamClient.Dispose();

            int count = receivedPairs.Count;

            await Task.Delay(1000);

            receivedPairs.Count.Should().Be(count);
        }

        /// <summary>
        /// tests subscription with 10 different pairs
        /// </summary>
        [Fact]
        public void test_multiple_pairs_stream()
        {
            Dictionary<string, List<IncomingCurrencyPair>> dictReceivedPairs = new Dictionary<string, List<IncomingCurrencyPair>>();

            foreach (var pair in _pairs)
            {
                Task.Run(async () =>
                {
                    string handlerName = $"{pair} handler";

                    List<IncomingCurrencyPair> receivedPairs = new List<IncomingCurrencyPair>();

                    var _streamClientPerPair = new KaikoExchangeCrossStreamHandler(_config,
                        new StreamRequestParameter(pair, StreamRequestParameterAggregate.OneHour),
                        handlerName);

                    _streamClientPerPair.Subscribe((IncomingCurrencyPair obj) =>
                    {
                        receivedPairs.Add(obj);

                        _output.WriteLine($"Observer: Price of {obj.Symbol} is {obj.Price}");
                    });

                    _streamClientPerPair.Start();

                    await Task.Delay(5000);

                    _streamClientPerPair.Stop();

                    _streamClientPerPair.Dispose();

                    dictReceivedPairs.Add(handlerName, receivedPairs);
                }).Wait();              
                

            }

            var min = dictReceivedPairs.Select(us => us.Value.Count).Min();

            min.Should().BeGreaterThan(0);
        }
    }
}