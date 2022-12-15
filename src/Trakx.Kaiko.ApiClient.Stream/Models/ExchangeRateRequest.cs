using System.Text.Json.Serialization;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Wrapper for <see cref="KaikoSdk.Stream.AggregatesSpotExchangeRateV1.StreamAggregatesSpotExchangeRateRequestV1"/>
/// and <see cref="KaikoSdk.Stream.AggregatesDirectExchangeRateV1.StreamAggregatesDirectExchangeRateRequestV1"/>
/// </summary>
public class ExchangeRateRequest
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("interval")]
    public AggregateInterval Interval { get; set; }

    /// <summary>
    /// Default parameterless constructor for JSON deserialization.
    /// </summary>
    public ExchangeRateRequest() { }

    public ExchangeRateRequest(string symbol, string currency, AggregateInterval interval = AggregateInterval.OneSecond)
    {
        Symbol = symbol;
        Currency = currency;
        Interval = interval;
    }
}
