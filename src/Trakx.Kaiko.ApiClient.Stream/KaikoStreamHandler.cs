using Grpc.Core;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using KaikoSdk;
using System.Collections.Generic;
using Trakx.Kaiko.ApiClient.Stream.Models;
using SpotClient = KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1.StreamAggregatesSpotExchangeRateServiceV1Client;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;

namespace Trakx.Kaiko.ApiClient.Stream
{
    public class KaikoStreamHandler : IKaikoStreamHandler
    {
        private readonly GrpcChannel _channel;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public KaikoStreamHandler(KaikoStreamConfiguration config)
        {
            if (config == null) return;
            if (string.IsNullOrWhiteSpace(config.ChannelUrl)) return;
            if (string.IsNullOrWhiteSpace(config.ApiKey)) return;

            ChannelCredentials credentials = CreateCredentials(config.ApiKey);
            GrpcChannelOptions options = CreateChannelOptions(credentials);
            _channel = GrpcChannel.ForAddress(config.ChannelUrl, options);

            _cancellationTokenSource = new CancellationTokenSource();
        }

        private static GrpcChannelOptions CreateChannelOptions(ChannelCredentials credentials)
        {
            return new GrpcChannelOptions
            {
                Credentials = credentials,
                ServiceConfig = new ServiceConfig
                {
                    MethodConfigs =
                    {
                        new MethodConfig
                        {
                            Names = { MethodName.Default },
                            RetryPolicy = SetupRetryPolicy()
                        }
                    }
                }
            };
        }

        private static RetryPolicy SetupRetryPolicy()
        {
            return new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(5),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable }
            };
        }

        private static ChannelCredentials CreateCredentials(string apiKey)
        {
            var interceptor = CallCredentials.FromInterceptor((_, metadata) =>
            {
                metadata.Add("Authorization", $"Bearer {apiKey}");
                return Task.CompletedTask;
            });

            return ChannelCredentials.Create(new SslCredentials(), interceptor);
        }

        public async IAsyncEnumerable<SpotExchangeRateResponse> StreamSpotExchangeRates(string symbol, string currency = "usd")
        {
            if (_channel == null) yield break;

            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(null, nameof(symbol));
            }
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException(null, nameof(currency));
            }

            var client = new SpotClient(_channel);

            var request = new StreamAggregatesSpotExchangeRateRequestV1
            {
                Code = $"{symbol}-{currency}",
                Aggregate = "1s"
            };

            var subscription = client.Subscribe(request, cancellationToken: _cancellationTokenSource.Token);

            await foreach (var current in subscription.ResponseStream.ReadAllAsync())
            {
                decimal.TryParse(current.Price, out var price);

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