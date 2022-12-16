using Trakx.Utils.Attributes;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Configuration to connect to the Kaiko Stream Grpc API.
/// </summary>
public record KaikoStreamConfiguration
{
#nullable disable

    /// <summary>
    /// The live url is currently <see cref="https://gateway-v0-grpc.kaiko.ovh"/>
    /// </summary>
    public string ChannelUrl { get; init; }

    [AwsParameter]
    [SecretEnvironmentVariable]
    public string ApiKey { get; init; }
#nullable restore
}