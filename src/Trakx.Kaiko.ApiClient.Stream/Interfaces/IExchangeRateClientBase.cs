namespace Trakx.Kaiko.ApiClient.Stream;

public interface IExchangeRateClientBase : IDisposable
{
    /// <summary>
    /// Stream results via <see cref="IObservable{T}"/>
    /// </summary>
    /// <param name="request">Options to define what data is streamed from the API.</param>
    /// <param name="cancellationToken">Allow cancellation from caller.</param>
    /// <returns></returns>
    IObservable<ExchangeRateResponse> Observe(ExchangeRateRequest request, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Stream results via <see cref="IAsyncEnumerable{T}"/>
    /// </summary>
    /// <param name="request">Options to define what data is streamed from the API.</param>
    /// <param name="cancellationToken">Allow cancellation from caller.</param>
    /// <returns></returns>
    IAsyncEnumerable<ExchangeRateResponse> Stream(ExchangeRateRequest request, CancellationToken? cancellationToken = default);
}
