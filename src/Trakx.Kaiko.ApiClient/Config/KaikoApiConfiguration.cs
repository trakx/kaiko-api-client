using Trakx.Common.Attributes;

namespace Trakx.Kaiko.ApiClient;

public record KaikoApiConfiguration
{
#nullable disable
    public Uri MarketDataBaseUrl { get; init; }

    public Uri ReferenceDataBaseUrl { get; init; }

    [AwsParameter(AllowGlobal = true)]
    public string ApiKey { get; init; }
#nullable restore
}
