using System.Text.Json.Serialization;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Wrapper for <see cref="KaikoSdk.Stream.AggregatesSpotExchangeRateV1.StreamAggregatesSpotExchangeRateResponseV1"/>
/// and <see cref="KaikoSdk.Stream.AggregatesDirectExchangeRateV1.StreamAggregatesDirectExchangeRateResponseV1"/>
/// </summary>
public class ExchangeRateResponse
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("code")]
    public DateTime Timestamp { get; set; }
}
