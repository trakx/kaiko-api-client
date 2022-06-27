using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using KaikoSdk;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using Microsoft.Extensions.Options;
using Serilog;
using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream
{
    /// <summary>
    /// This service provides the price of any asset quoted in a Fiat currency within Open Exchange Rates.
    /// </summary>
    public class KaikoExchangeCrossStreamHandler : KaikoStreamHandlerBase, IKaikoStreamHandler
    {
        private readonly Channel _channel;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly KaikoApiConfiguration _configuration;
        private readonly StreamAggregatesSpotExchangeRateServiceV1Client _clientService;
        private readonly StreamRequestParameter _streamRequestParameter;
        private readonly string _handlerName;

        private static readonly ILogger Logger = Log.Logger.ForContext(MethodBase.GetCurrentMethod()!.DeclaringType!);


        public KaikoExchangeCrossStreamHandler(
            IOptions<KaikoApiConfiguration> configuration,
            StreamRequestParameter streamRequestParameter,
            string handlerName = "") 
        {
            _configuration = configuration.Value;
            _streamRequestParameter = streamRequestParameter;
            _cancellationTokenSource = new CancellationTokenSource();
            _handlerName = handlerName;

            _channel = ChannelProvider.Create(_configuration.TargetChannel, _configuration.ApiKey);
            _clientService = new StreamAggregatesSpotExchangeRateServiceV1Client(_channel);
        }

        /// <summary>
        /// Start the streaming
        /// </summary>
        public async void Start()
        {
            try
            {
                var req = new StreamAggregatesSpotExchangeRateRequestV1
                {
                    Code = _streamRequestParameter.Code,
                    Aggregate = _streamRequestParameter.Aggregate.Value
                };

                var reply = _clientService.Subscribe(req, null, null, _cancellationTokenSource.Token);

                var stream = reply.ResponseStream;

                while (await stream.MoveNext())
                {
                    StreamAggregatesSpotExchangeRateResponseV1 response = stream.Current;

                    UpdatePrice(response.Code, response.Price);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error while subscribing to Kaiko Spot Exchange Rate stream service");
            }
        }

        /// <summary>
        /// Stop the stream
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();

            Logger.Debug("Spot Exchange data subscription stopped");
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
