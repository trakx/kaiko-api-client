﻿using Trakx.Utils.Attributes;

namespace Trakx.Kaiko.ApiClient;

public record KaikoApiConfiguration
{
#nullable disable
    public Uri BaseUrl { get; init; }

    [AwsParameter]
    public string ApiKey { get; init; }
#nullable restore
}