using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Represents the Trakx client to consume <see href="https://sdk.kaiko.com/#kaiko-stream-streamaggregatesspotexchangerateservicev1"/>
/// </summary>
public interface ISpotExchangeRatesClient
{
    /// <summary>
    /// Stream results via <see cref="IObservable{T}"/>
    /// </summary>
    /// <param name="request">Options to define what data is streamed from the API.</param>
    /// <param name="cancellationToken">Allow cancellation from caller.</param>
    /// <returns></returns>
    IObservable<ExchangeRateResponse> Observe(ExchangeRateRequest request, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Stream results via <see cref="IAsyncEnumerable{T}"/>
    /// </summary>
    /// <param name="request">Options to define what data is streamed from the API.</param>
    /// <param name="cancellationToken">Allow cancellation from caller.</param>
    /// <returns></returns>
    IAsyncEnumerable<ExchangeRateResponse> Stream(ExchangeRateRequest request, CancellationToken? cancellationToken = default);
}