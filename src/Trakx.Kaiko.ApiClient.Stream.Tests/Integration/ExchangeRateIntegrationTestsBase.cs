﻿using System.Reactive.Linq;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

[Collection(nameof(ApiTestCollection))]
public class ExchangeRateIntegrationTestsBase<TClient>
    where TClient : IExchangeRateClientBase
{
    private const int RunSeconds = 3;

    protected readonly ITestOutputHelper Output;
    protected readonly ServiceProvider Services;

    public ExchangeRateIntegrationTestsBase(KaikoStreamFixture fixture, ITestOutputHelper output)
    {
        Output = output;
        Services = fixture.ServiceProvider;
    }

    protected async Task<int> StreamAsync(string symbol, string currency, StatusCode expectedStatus)
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(RunSeconds));
        var replies = 0;

        try
        {
            using var client = Services.GetRequiredService<TClient>();
            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);
            await foreach (var response in client.Stream(request, cancellation.Token).ConfigureAwait(false))
            {
                AssertResponse(response, symbol, currency);
                replies++;
            }
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().BeOneOf(expectedStatus, StatusCode.PermissionDenied);
        }

        return replies;
    }

    protected async Task<int> ObserveAsync(string symbol, string currency, StatusCode expectedStatus)
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(RunSeconds));
        var replies = 0;

        try
        {
            void OnNext(ExchangeRateResponse response)
            {
                AssertResponse(response, symbol, currency);
                replies++;
            }

            void OnError(Exception x)
            {
                Output.WriteLine(x.Message);
            }

            using var client = Services.GetRequiredService<TClient>();

            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);
            await client
                .Observe(request, cancellation.Token)
                .Do(OnNext, OnError)
                .LastOrDefaultAsync();
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().BeOneOf(expectedStatus, StatusCode.PermissionDenied);
        }

        return replies;
    }

    protected void AssertResponse(ExchangeRateResponse response, string expectedSymbol, string expectedCurrency)
    {
        response.Should().NotBeNull();
        response.BaseSymbol.Should().Be(expectedSymbol);
        response.QuoteSymbol.Should().Be(expectedCurrency);
        Output.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff}:{1}", response.Timestamp, response.Price);
    }

    protected void AssertReplies(bool serviceEnabled, int replies)
    {
        if (serviceEnabled)
        {
            replies.Should().BeGreaterThan(0);
        }
        else
        {
            replies.Should().Be(0);
        }
    }

}


[CollectionDefinition(nameof(ApiTestCollection), DisableParallelization = true)]
public class ApiTestCollection : ICollectionFixture<KaikoStreamFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
