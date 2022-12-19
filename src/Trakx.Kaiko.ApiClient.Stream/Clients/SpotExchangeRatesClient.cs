using System.Collections.Generic;
using Grpc.Core;
using KaikoSdk.Stream.AggregatesDirectExchangeRateV1;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using Microsoft.IdentityModel.Tokens;
using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream;

/// Trakx client implementation for
/// <see cref="StreamAggregatesSpotExchangeRateServiceV1Client"/>
public class SpotExchangeRatesClient : KaikoStreamClientBase<ExchangeRateRequest, ExchangeRateResponse>, ISpotExchangeRatesClient
{
    private readonly StreamAggregatesSpotExchangeRateServiceV1Client _client;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public SpotExchangeRatesClient(StreamAggregatesSpotExchangeRateServiceV1Client client)
    {
        _client = client;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override void ValidateRequest(ExchangeRateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        if (string.IsNullOrWhiteSpace(request.Symbol))
        {
            throw new ArgumentException($"{nameof(request.Symbol)} property cannot be null or empty", nameof(request));
        }
        if (string.IsNullOrWhiteSpace(request.Currency))
        {
            throw new ArgumentException($"{nameof(request.Currency)} property cannot be null or empty", nameof(request));
        }
    }

    internal override async IAsyncEnumerable<ExchangeRateResponse> StreamInternalAsync(
        ExchangeRateRequest request, CancellationToken? cancellationToken)
    {
        var apiRequest = new StreamAggregatesSpotExchangeRateRequestV1
        {
            Code = $"{request.Symbol}-{request.Currency}",
            Aggregate = request.Interval.ToApiParameter()
        };

        var token = cancellationToken ?? _cancellationTokenSource.Token;

        var subscription = _client.Subscribe(apiRequest, cancellationToken: token);

        await foreach (var current in subscription.ResponseStream.ReadAllAsync(cancellationToken: token))
        {
            var price = TryParseDecimal(current.Price);
            if (price == null) continue;

            var codeParts = current.Code.Split('-');

            var item = new ExchangeRateResponse
            {
                Price = price.Value,
                Symbol = codeParts.ElementAtOrDefault(0),
                Currency = codeParts.ElementAtOrDefault(1),
                Timestamp = current.Timestamp.ToDateTimeOffset(),
            };

            yield return item;
        }
    }
}