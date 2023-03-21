﻿using System.Text.Json.Serialization;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary> Wrapper for <see cref="KaikoSdk.Stream.MarketUpdateV1.StreamMarketUpdateRequestV1"/></summary>
public class MarketUpdateRequest
{
    [JsonPropertyName("baseSymbols")]
    public string[] BaseSymbols { get; set; } = Array.Empty<string>();

    [JsonPropertyName("quoteSymbols")]
    public string[] QuoteSymbols { get; set; } = Array.Empty<string>();

    [JsonPropertyName("exchanges")]
    public string[] Exchanges { get; set; } = Array.Empty<string>();

    [JsonPropertyName("allUpdates")]
    public bool IncludeAllUpdates { get; set; }

    [JsonPropertyName("topOfBook")]
    public bool IncludeTopOfBook { get; set; }

    [JsonPropertyName("fullBook")]
    public bool IncludeFullBook { get; set; }

    [JsonPropertyName("trades")]
    public bool IncludeTrades { get; set; }
}
