using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class SpotExchangeRateClientTests
{
    [Fact]
    public void StreamAsync_expects_request()
    {
        ExchangeRateRequest? request = null;

        var action = SetupStream(request!);

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }

    [Fact]
    public void StreamAsync_expects_symbol()
    {
        var request = new ExchangeRateRequest();

        var action = SetupStream(request!);

        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(request))
            .WithMessage("*symbol*");
    }

    [Fact]
    public void StreamAsync_expects_currency()
    {
        var request = new ExchangeRateRequest { Symbol = "btc" };

        var action = SetupStream(request!);

        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(request))
            .WithMessage("*currency*");
    }

    private static Func<IAsyncEnumerable<ExchangeRateResponse>> SetupStream(ExchangeRateRequest request)
    {
        var sdkClient = Substitute.For<StreamAggregatesSpotExchangeRateServiceV1Client>();
        var client = new SpotExchangeRatesClient(sdkClient);
        var action = () => client.Stream(request);
        return action;
    }
}
