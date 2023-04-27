using System.Text.Json.Serialization;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary> Wrapper for <see cref="KaikoSdk.Stream.MarketUpdateV1.StreamMarketUpdateResponseV1"/> </summary>
public class MarketUpdateResponse
{
    [JsonPropertyName("baseSymbol")]
    public string BaseSymbol { get; set; } = string.Empty;

    [JsonPropertyName("quoteSymbol")]
    public string QuoteSymbol { get; set; } = string.Empty;

    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("timestampExchange")]
    public DateTimeOffset? TimestampExchange { get; set; }

    [JsonPropertyName("timestampCollection")]
    public DateTimeOffset? TimestampCollection { get; set; }

    [JsonPropertyName("updateType")]
    public string UpdateType { get; set; } = string.Empty;

    [JsonPropertyName("sequenceId")]
    public string SequenceId { get; set; } = string.Empty;
}
