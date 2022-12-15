using System;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

[Collection(nameof(ApiTestCollection))]
public class KaikoStreamTestsBase
{
    protected readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected readonly ServiceProvider Services;

    public KaikoStreamTestsBase(KaikoStreamFixture fixture, ITestOutputHelper output)
    {
        Output = output;
        Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();
        Services = fixture.Services;
    }
}


[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<KaikoStreamFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
