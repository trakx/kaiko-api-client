using System.Collections.Generic;
using System.Globalization;
using Grpc.Core;
using Grpc.Net.Client;
using KaikoSdk;
using KaikoSdk.Stream.AggregatesSpotExchangeRateV1;
using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Base class for all implementations for Kaiko Stream Exchange Rate services.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class KaikoStreamClientBase<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TResponse> StreamAsync(TRequest request, CancellationToken? cancellationToken = default)
    {
        ValidateRequest(request);
        return StreamInternalAsync(request, cancellationToken);
    }

    /// <summary>
    /// Validate the request and its properties.
    /// </summary>
    /// <param name="request"></param>
    protected abstract void ValidateRequest(TRequest request);

    /// <summary>
    /// Private implementation of <see cref="StreamAsync"/>
    /// Split from <see cref="StreamAsync"/> to ensure parameters in <see cref="TRequest"/> 
    /// are validated immediately, before subscribing to the stream.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal abstract IAsyncEnumerable<TResponse> StreamInternalAsync(TRequest request, CancellationToken? cancellationToken);

    protected static decimal? TryParseDecimal(string text)
    {
        var valid = decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value);
        if (!valid) return null;
        return value;
    }
}