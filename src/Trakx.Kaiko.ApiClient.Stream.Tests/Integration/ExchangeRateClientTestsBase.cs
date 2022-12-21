using System.Reactive.Linq;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

[Collection(nameof(ApiTestCollection))]
public partial class ExchangeRateClientTestsBase<TClient>
    where TClient : IExchangeRateClientBase
{
    private const int RunSeconds = 2;

    protected readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected readonly ServiceProvider Services;
    protected readonly IDirectExchangeRatesClient _client;

    public ExchangeRateClientTestsBase(KaikoStreamFixture fixture, ITestOutputHelper output)
    {
        Output = output;
        Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();
        Services = fixture.Services;
        _client = fixture.Services.GetRequiredService<IDirectExchangeRatesClient>();
    }

    protected async Task<int> StreamAsync(string symbol, string currency, StatusCode expectedStatus)
    {
        var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(RunSeconds));
        var replies = 0;

        try
        {
            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);
            await foreach (var response in _client.Stream(request, cancellation.Token).ConfigureAwait(false))
            {
                AssertResponse(response, symbol, currency);
                replies++;
            }
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().Be(expectedStatus);
        }

        return replies;
    }

    protected async Task<int> ObserveAsync(string symbol, string currency, StatusCode expectedStatus)
    {
        var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(RunSeconds));
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

            var request = new ExchangeRateRequest(symbol, currency, interval: AggregateInterval.OneSecond);
            await _client
                .Observe(request, cancellation.Token)
                .Do(OnNext, OnError)
                .LastOrDefaultAsync();
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().Be(expectedStatus);
        }

        return replies;
    }

    protected void AssertResponse(ExchangeRateResponse response, string expectedSymbol, string expectedCurrency)
    {
        response.Should().NotBeNull();
        response.Symbol.Should().Be(expectedSymbol);
        response.Currency.Should().Be(expectedCurrency);
        Output.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff}:{1}", response.Timestamp, response.Price);
    }
}


[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<KaikoStreamFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
