namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Stream data from <see cref="https://sdk.kaiko.com/#kaiko-stream-streammarketupdateservicev1"/>
/// </summary>
public interface IMarketUpdateClient
{
    /// <summary><inheritdoc cref="IMarketUpdateClient"/> via <see cref="IObservable{T}"/></summary>
    /// <param name="request">Options to define what data is streamed from the API.</param>
    /// <param name="cancellationToken">Allow cancellation from caller.</param>
    IObservable<MarketUpdateResponse> Observe(MarketUpdateRequest request, CancellationToken cancellationToken = default);

    /// <summary><inheritdoc cref="IMarketUpdateClient"/> via <see cref="IAsyncEnumerable{T}"/></summary>
    /// <param name="request">Options to define what data is streamed from the API.</param>
    /// <param name="cancellationToken">Allow cancellation from caller.</param>
    IAsyncEnumerable<MarketUpdateResponse> Stream(MarketUpdateRequest request, CancellationToken cancellationToken = default);
}
