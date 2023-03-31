using Grpc.Core;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream;

/// Trakx client implementation for <see cref="StreamAggregatesSpotExchangeRateServiceV1Client"/>
public class SpotExchangeRatesClient : ExchangeRateClientBase<StreamAggregatesSpotExchangeRateResponseV1>, ISpotExchangeRatesClient
{
    private readonly StreamAggregatesSpotExchangeRateServiceV1Client _client;

    public SpotExchangeRatesClient(StreamAggregatesSpotExchangeRateServiceV1Client client)
    {
        _client = client;
    }

    protected override AsyncServerStreamingCall<StreamAggregatesSpotExchangeRateResponseV1> Subscribe(
        ExchangeRateRequest request, CancellationToken token)
    {
        var apiRequest = new StreamAggregatesSpotExchangeRateRequestV1
        {
            Code = $"{request.Symbol}-{request.Currency}",
            Aggregate = request.Interval.ToApiParameter()
        };

        var subscription = _client.Subscribe(apiRequest, cancellationToken: token);
        return subscription;
    }

    protected override ExchangeRateResponse? BuildResponse(StreamAggregatesSpotExchangeRateResponseV1 current)
    {
        if (current == null) return null;

        var price = TryParseDecimal(current.Price);
        if (price == null) return null;

        var codeParts = current.Code.Split('-');
        if (codeParts.Length != 2) return null;

        return new ExchangeRateResponse
        {
            Price = price.Value,
            BaseSymbol = codeParts[0],
            QuoteSymbol = codeParts[1],
            Timestamp = current.Timestamp.ToDateTimeOffset(),
        };
    }
}
