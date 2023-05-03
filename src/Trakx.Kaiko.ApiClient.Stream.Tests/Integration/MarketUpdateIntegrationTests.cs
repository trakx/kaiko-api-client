using System.Reactive.Linq;
using System.Text.Json;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

[Collection(nameof(ApiTestCollection))]
public class MarketUpdateIntegrationTests
{
    private static readonly TimeSpan TestDuration = TimeSpan.FromSeconds(10);

    private readonly ITestOutputHelper _output;
    private readonly IMarketUpdateClient _client;
    private readonly MarketUpdateRequest _request;

    public MarketUpdateIntegrationTests(KaikoStreamFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _client = fixture.ServiceProvider.GetRequiredService<IMarketUpdateClient>();

        _request = new MarketUpdateRequest
        {
            Exchanges = new[] { "cbse" },
            BaseSymbols = new[] { "btc", "eth" },
            QuoteSymbols = new[] { "usd" },
            IncludeTopOfBook = true,
        };
    }

    [Fact]
    public async Task Stream_should_handle_unknown_exchange()
    {
        _request.Exchanges = new[] { "invalidExchange" };
        var replies = await StreamAsync(StatusCode.FailedPrecondition);
        replies.Should().Be(0);
    }

    [Fact]
    public async Task Observe_should_handle_unknown_exchange()
    {
        _request.Exchanges = new[] { "invalidExchange" };
        var replies = await ObserveAsync(StatusCode.FailedPrecondition);
        replies.Should().Be(0);
    }

    [InlineData(EnabledServices.MarketUpdate)]
    [Theory]
    public async Task Stream_should_return_prices(bool serviceEnabled)
    {
        var replies = await StreamAsync();
        AssertReplies(serviceEnabled, replies);
    }

    [InlineData(EnabledServices.MarketUpdate)]
    [Theory]
    public async Task Observe_should_return_prices(bool serviceEnabled)
    {
        var replies = await ObserveAsync();
        AssertReplies(serviceEnabled, replies);
    }

    private async Task<int> StreamAsync(StatusCode expectedStatus = StatusCode.Cancelled, int maxReplies = 3)
    {
        using var cancellation = new CancellationTokenSource(TestDuration);
        var replies = 0;

        try
        {
            var stream = _client.Stream(_request, cancellation.Token);
            await foreach (var response in stream)
            {
                var json = JsonSerializer.Serialize(response);
                _output.WriteLine(json);
                replies++;
                if (replies == maxReplies) cancellation.Cancel();
            }
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().BeOneOf(expectedStatus, StatusCode.PermissionDenied);
        }

        return replies;
    }

    private async Task<int> ObserveAsync(StatusCode expectedStatus = StatusCode.Cancelled, int maxReplies = 3)
    {
        using var cancellation = new CancellationTokenSource(TestDuration);
        var replies = 0;

        try
        {
            void OnNext(MarketUpdateResponse response)
            {
                AssertResponse(response);
                replies++;
                if (replies == maxReplies) cancellation.Cancel();
            }

            void OnError(Exception x)
            {
                _output.WriteLine(x.Message);
            }

            await _client
                .Observe(_request, cancellation.Token)
                .Do(OnNext, OnError)
                .LastOrDefaultAsync();
        }
        catch (RpcException x)
        {
            x.StatusCode.Should().BeOneOf(expectedStatus, StatusCode.PermissionDenied);
        }

        return replies;
    }

    private void AssertResponse(MarketUpdateResponse response)
    {
        response.Should().NotBeNull();
        _request.BaseSymbols.Should().Contain(response.BaseSymbol);
        _request.QuoteSymbols.Should().Contain(response.QuoteSymbol);
        _request.Exchanges.Should().Contain(response.Exchange);

        _output.WriteLine("{0:HH:mm:ss.fff} : {1} : {2}-{3} : {4}",
            response.Timestamp, response.Exchange,
            response.BaseSymbol, response.QuoteSymbol,
            response.Price);
    }

    private static void AssertReplies(bool serviceEnabled, int replies)
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
