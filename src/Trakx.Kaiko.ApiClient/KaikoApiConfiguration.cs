using Trakx.Utils.Attributes;

namespace Trakx.Kaiko.ApiClient;

public record KaikoApiConfiguration
{
#nullable disable
    public string BaseUrl { get; init; }

    [AwsParameter]
    [SecretEnvironmentVariable]
    public string ApiKey { get; init; }
#nullable restore
}