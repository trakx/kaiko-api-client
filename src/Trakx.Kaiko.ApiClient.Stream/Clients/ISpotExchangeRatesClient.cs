namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Represents the Trakx client to consume <see href="https://sdk.kaiko.com/#kaiko-stream-streamaggregatesspotexchangerateservicev1"/>
/// </summary>
public interface ISpotExchangeRatesClient
{
    /// <summary>
    /// Client for 
    /// </summary>
    /// <param name="request">Options to define what data is streamed from the API.</param>
    /// <param name="cancellationToken">Allow cancellation from caller</param>
    /// <returns></returns>
    IAsyncEnumerable<ExchangeRateResponse> StreamAsync(ExchangeRateRequest request, CancellationToken? cancellationToken = default);
}