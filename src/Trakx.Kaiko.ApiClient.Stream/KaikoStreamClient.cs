using System.Globalization;
using Grpc.Core;
using Grpc.Net.Client;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using Trakx.Kaiko.ApiClient.Stream.Models;
using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream
{
    public partial class KaikoStreamClient : IKaikoStreamClient
    {
        private readonly StreamAggregatesSpotExchangeRateServiceV1Client _client;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public KaikoStreamClient(StreamAggregatesSpotExchangeRateServiceV1Client client)
        {
            _client = client;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async IAsyncEnumerable<SpotExchangeRateResponse> StreamSpotExchangeRates(
            string symbol,
            string currency = "usd",
            CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(null, nameof(symbol));
            }
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException(null, nameof(currency));
            }

            var request = new StreamAggregatesSpotExchangeRateRequestV1
            {
                Code = $"{symbol}-{currency}",
                Aggregate = "1s"
            };

            var token = cancellationToken ?? _cancellationTokenSource.Token;

            var subscription = _client.Subscribe(request, cancellationToken: token);

            await foreach (var current in subscription.ResponseStream.ReadAllAsync(cancellationToken: token))
            {
                decimal.TryParse(current.Price, NumberStyles.Number, CultureInfo.InvariantCulture, out var price);

                var item = new SpotExchangeRateResponse
                {
                    Code = current.Code,
                    Price = price,
                    Timestamp = current.Timestamp.ToDateTime(),
                };

                yield return item;
            }
        }
    }
}