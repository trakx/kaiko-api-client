using Trakx.Common.Attributes;

namespace Trakx.Kaiko.ApiClient;

public record KaikoApiConfiguration
{
    public required Uri MarketDataBaseUrl { get; init; }

    public required Uri ReferenceDataBaseUrl { get; init; }

    [AwsParameter(AllowGlobal = true)]
    public required string ApiKey { get; init; }
}