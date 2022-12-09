using MathNet.Numerics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Contrib.WaitAndRetry;
using Polly;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Kaiko.ApiClient.Stream;

public static partial class AddKaikoStreamExtensions
{
    public static IServiceCollection AddKaikoStream(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(KaikoStreamConfiguration)).Get<KaikoStreamConfiguration>();
        services.AddKaikoStream(config);
        return services;
    }

    public static IServiceCollection AddKaikoStream(this IServiceCollection services, KaikoStreamConfiguration streamConfiguration)
    {
        services.AddSingleton(streamConfiguration);
        services.AddSingleton<IKaikoStreamHandler, KaikoStreamHandler>();
        return services;
    }
}