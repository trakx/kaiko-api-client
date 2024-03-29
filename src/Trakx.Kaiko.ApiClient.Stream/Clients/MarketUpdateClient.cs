﻿using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Grpc.Core;
using KaikoSdk.Core;
using KaikoSdk.Stream.MarketUpdateV1;
using static KaikoSdk.StreamMarketUpdateServiceV1;
using SdkUpdateType = KaikoSdk.Stream.MarketUpdateV1.StreamMarketUpdateResponseV1.Types.StreamMarketUpdateType;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Client for Kaiko Stream Market Update service, <see cref="https://sdk.kaiko.com/#kaiko-stream-streammarketupdateservicev1"/>
/// </summary>
public class MarketUpdateClient : IMarketUpdateClient
{
    private readonly StreamMarketUpdateServiceV1Client _client;

    internal const string AllExchangesWildcard = "*";

    public MarketUpdateClient(StreamMarketUpdateServiceV1Client client)
    {
        _client = client;
    }

    /// <inheritdoc/>
    public IObservable<MarketUpdateResponse> Observe(MarketUpdateRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var subscription = Subscribe(request, cancellationToken);

        var serverStream = subscription.ResponseStream;

        var observableStream = new ObservableStream<StreamMarketUpdateResponseV1>(serverStream, cancellationToken);

        var responseStream = observableStream
            .Select(BuildResponse)
            .Where(p => p != null)
            .Select(p => p!);

        return responseStream;
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<MarketUpdateResponse> Stream(MarketUpdateRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);
        return StreamInternalAsync(request, cancellationToken);
    }

    internal const string MissingBaseSymbolsError = "Base symbols must be defined.";
    internal const string MissingQuoteSymbolsError = "Quote symbols must be defined.";
    internal const string MissingDataTypeError = "At least one type of data (order book or trades) should be included.";

    /// <summary>
    /// Validate the request and its properties.
    /// </summary>
    /// <param name="request"></param>
    protected virtual void ValidateRequest(MarketUpdateRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.BaseSymbols.IsNullOrEmpty())
        {
            throw new ArgumentException(MissingBaseSymbolsError);
        }
        if (request.QuoteSymbols.IsNullOrEmpty())
        {
            throw new ArgumentException(MissingQuoteSymbolsError);
        }

        var somethingIncluded = request.IncludeTrades || request.IncludeTopOfBook || request.IncludeFullBook;

        if (!somethingIncluded!)
        {
            throw new ArgumentException(MissingDataTypeError);
        }
    }

    /// <summary>
    /// Private implementation of <see cref="Stream"/>
    /// Split from <see cref="Stream"/> to ensure parameters in <see cref="MarketUpdateRequest"/> 
    /// are validated immediately, before subscribing to the stream.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal virtual async IAsyncEnumerable<MarketUpdateResponse> StreamInternalAsync(
        MarketUpdateRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var subscription = Subscribe(request, cancellationToken);

        var stream = subscription.ResponseStream;

        await foreach (var current in stream.ReadAllAsync(cancellationToken).ConfigureAwait(false))
        {
            var response = BuildResponse(current);
            if (response == null) continue;
            yield return response;
        }
    }

    private AsyncServerStreamingCall<StreamMarketUpdateResponseV1> Subscribe(MarketUpdateRequest request, CancellationToken cancellationToken)
    {
        var tradePairs = DefineTradePairs(request.BaseSymbols, request.QuoteSymbols);

        var exchanges = DefineExchanges(request.Exchanges);

        var apiRequest = new StreamMarketUpdateRequestV1
        {
            InstrumentCriteria = new KaikoSdk.Core.InstrumentCriteria
            {
                InstrumentClass = "spot",
                Code = string.Join(',', tradePairs),
                Exchange = exchanges,
            }
        };

        apiRequest.Commodities.AddRange(DefineCommodities(request));

        var subscription = _client.Subscribe(apiRequest, cancellationToken: cancellationToken);
        return subscription;
    }

    private static string DefineExchanges(string[] exchanges)
    {
        if (exchanges.IsNullOrEmpty()) return AllExchangesWildcard; // all supported exchanges
        return string.Join(',', exchanges);
    }

    private MarketUpdateResponse? BuildResponse(StreamMarketUpdateResponseV1 current)
    {
        if (current == null) return null;

        var codeParts = current.Code.Split('-');
        if (codeParts.Length != 2) return null;

        return new MarketUpdateResponse
        {
            BaseSymbol = codeParts[0],
            QuoteSymbol = codeParts[1],
            Exchange = current.Exchange,

            Price = (decimal)current.Price,
            Amount = (decimal)current.Amount,

            Timestamp = current.TsEvent.ToDateTimeOffset(),
            TimestampExchange = GetTimestamp(current.TsExchange),
            TimestampCollection = GetTimestamp(current.TsCollection),

            UpdateType = current.UpdateType.ToClientEnum(),
            SequenceId = current.SequenceId,
        };
    }

    internal static IEnumerable<StreamMarketUpdateCommodity> DefineCommodities(MarketUpdateRequest request)
    {
        if (request.IncludeTrades) yield return StreamMarketUpdateCommodity.SmucTrade;
        if (request.IncludeTopOfBook) yield return StreamMarketUpdateCommodity.SmucTopOfBook;
        if (request.IncludeFullBook) yield return StreamMarketUpdateCommodity.SmucFullOrderBook;
    }

    internal static IEnumerable<string> DefineTradePairs(string[] baseSymbols, string[] quoteSymbols)
    {
        foreach (var quoteSymbol in quoteSymbols)
        {
            foreach (var baseSymbol in baseSymbols)
            {
                yield return $"{baseSymbol}-{quoteSymbol}";
            }
        }
    }

    private static DateTimeOffset? GetTimestamp(TimestampValue value)
    {
        if (value == null) return null;
        return value.Value.ToDateTimeOffset();
    }
}

internal static class MarketUpdateClientExtensions
{
    internal static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
    {
        return items == null || !items.Any();
    }

    internal static StreamMarketUpdateType ToClientEnum(this StreamMarketUpdateResponseV1.Types.StreamMarketUpdateType updateType)
    {
        return SdkUpdateTypesToClient.GetValueOrDefault(updateType, StreamMarketUpdateType.Unknown);
    }

    private static readonly Dictionary<SdkUpdateType, StreamMarketUpdateType> SdkUpdateTypesToClient = new()
    {
        [SdkUpdateType.Unknown] = StreamMarketUpdateType.Unknown,
        [SdkUpdateType.TradeBuy] = StreamMarketUpdateType.TradeBuy,
        [SdkUpdateType.TradeSell] = StreamMarketUpdateType.TradeSell,
        [SdkUpdateType.BestAsk] = StreamMarketUpdateType.BestAsk,
        [SdkUpdateType.BestBid] = StreamMarketUpdateType.BestBid,
        [SdkUpdateType.UpdatedAsk] = StreamMarketUpdateType.UpdatedAsk,
        [SdkUpdateType.UpdatedBid] = StreamMarketUpdateType.UpdatedBid,
        [SdkUpdateType.Snapshot] = StreamMarketUpdateType.Snapshot,
        [SdkUpdateType.ForceSnapshot] = StreamMarketUpdateType.ForceSnapshot,
    };
}
