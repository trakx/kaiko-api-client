using Trakx.Utils.Attributes;

namespace Trakx.Kaiko.ApiClient;

public record KaikoStreamConfiguration
{
#nullable disable
    public string ChannelUrl { get; init; }

    [AwsParameter]
    [SecretEnvironmentVariable]
    public string ApiKey { get; init; }
#nullable restore
}