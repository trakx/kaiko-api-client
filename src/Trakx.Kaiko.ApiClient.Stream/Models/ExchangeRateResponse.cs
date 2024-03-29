﻿using System.Text.Json.Serialization;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Wrapper for <see cref="KaikoSdk.Stream.AggregatesSpotExchangeRateV1.StreamAggregatesSpotExchangeRateResponseV1"/>
/// and <see cref="KaikoSdk.Stream.AggregatesDirectExchangeRateV1.StreamAggregatesDirectExchangeRateResponseV1"/>
/// </summary>
public class ExchangeRateResponse
{
    [JsonPropertyName("baseSymbol")]
    public string? BaseSymbol { get; set; }

    [JsonPropertyName("quoteSymbol")]
    public string? QuoteSymbol { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }
}
