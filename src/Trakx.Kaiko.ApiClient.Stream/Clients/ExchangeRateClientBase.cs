using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Linq;
using System.Threading;
using Grpc.Core;
using Grpc.Net.Client;
using KaikoSdk;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Base class for all implementations for Kaiko Stream Aggregate Exchange Rate services.
/// </summary>
/// <typeparam name="TKaikoResponse">The Response type received from the Kaiko SDK client.</typeparam>
public class ExchangeRateClientBase<TKaikoResponse> : IExchangeRateClientBase
{
    private readonly CancellationTokenSource _cancellationSource;

    protected ExchangeRateClientBase()
    {
        _cancellationSource = new CancellationTokenSource();
    }

    /// <inheritdoc/>
    public virtual IObservable<ExchangeRateResponse> Observe(ExchangeRateRequest request, CancellationToken? cancellationToken = default)
    {
        ValidateRequest(request);

        var token = cancellationToken ?? _cancellationSource.Token;

        var serverSubscription = Subscribe(request, token);
        var serverStream = serverSubscription.ResponseStream;

        var observableStream = new ObservableStream<TKaikoResponse>(serverStream, token);

        var responseStream = observableStream
            .Select(BuildResponse)
            .Where(p => p != null)
            .Select(p => p!);

        return responseStream;
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<ExchangeRateResponse> Stream(ExchangeRateRequest request, CancellationToken? cancellationToken = default)
    {
        ValidateRequest(request);
        return StreamInternalAsync(request, cancellationToken);
    }

    /// <summary>
    /// Validate the request and its properties.
    /// </summary>
    /// <param name="request"></param>
    protected virtual void ValidateRequest(ExchangeRateRequest request)
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

    /// <summary>
    /// Private implementation of <see cref="Stream"/>
    /// Split from <see cref="Stream"/> to ensure parameters in <see cref="ExchangeRateRequest"/> 
    /// are validated immediately, before subscribing to the stream.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal virtual async IAsyncEnumerable<ExchangeRateResponse> StreamInternalAsync(
        ExchangeRateRequest request, CancellationToken? cancellationToken)
    {
        var token = cancellationToken ?? _cancellationSource.Token;

        var subscription = Subscribe(request, token);

        var stream = subscription.ResponseStream;

        await foreach (var current in stream.ReadAllAsync(token).ConfigureAwait(false))
        {
            var response = BuildResponse(current);
            if (response == null) continue;
            yield return response;
        }
    }

    protected virtual AsyncServerStreamingCall<TKaikoResponse> Subscribe(ExchangeRateRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    protected virtual ExchangeRateResponse? BuildResponse(TKaikoResponse current)
    {
        throw new NotImplementedException();
    }

    protected static decimal? TryParseDecimal(string text)
    {
        var valid = decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value);
        if (!valid) return null;
        return value;
    }


    #region IDisposable

    private bool _wasDisposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_wasDisposed) return;
        if (disposing)
        {
            if (!_cancellationSource.IsCancellationRequested)
            {
                _cancellationSource.Cancel();
            }
            _cancellationSource.Dispose();
        }
        _wasDisposed = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
