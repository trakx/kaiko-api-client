using System.Collections.Generic;
using Grpc.Core;
using KaikoSdk.Stream.AggregatesDirectExchangeRateV1;
using static KaikoSdk.StreamAggregatesDirectExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream;

/// Trakx client implementation for <see cref="StreamAggregatesDirectExchangeRateServiceV1Client"/>
public class DirectExchangeRatesClient : ExchangeRateClientBase<StreamAggregatesDirectExchangeRateResponseV1>, IDirectExchangeRatesClient
{
    private readonly StreamAggregatesDirectExchangeRateServiceV1Client _client;

    public DirectExchangeRatesClient(StreamAggregatesDirectExchangeRateServiceV1Client client)
        : base()
    {
        _client = client;
    }

    protected override AsyncServerStreamingCall<StreamAggregatesDirectExchangeRateResponseV1> Subscribe(
        ExchangeRateRequest request, CancellationToken token)
    {
        var apiRequest = new StreamAggregatesDirectExchangeRateRequestV1
        {
            Code = $"{request.Symbol}-{request.Currency}",
            Aggregate = request.Interval.ToApiParameter()
        };

        var subscription = _client.Subscribe(apiRequest, cancellationToken: token);
        return subscription;
    }

    protected override ExchangeRateResponse? BuildResponse(StreamAggregatesDirectExchangeRateResponseV1 current)
    {
        if (current == null) return null;

        var price = TryParseDecimal(current.Price);
        if (price == null) return null;

        var codeParts = current.Code.Split('-');

        return new ExchangeRateResponse
        {
            Price = price.Value,
            Symbol = codeParts.ElementAtOrDefault(0),
            Currency = codeParts.ElementAtOrDefault(1),
            Timestamp = current.Timestamp.ToDateTimeOffset(),
        };
    }
}