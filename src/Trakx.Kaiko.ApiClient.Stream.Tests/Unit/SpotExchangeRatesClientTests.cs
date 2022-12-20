using static KaikoSdk.StreamAggregatesSpotExchangeRateServiceV1;

#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class SpotExchangeRatesClientTests
{
    [Fact]
    public void StreamAsync_expects_request()
    {
        ExpectException(client =>
        {
            client.Stream(null);
        }
        , exception =>
        {
            exception.Should().BeOfType<ArgumentNullException>();
            exception.As<ArgumentNullException>().ParamName.Should().Be("request");
        });
    }

    [Fact]
    public void StreamAsync_expects_symbol()
    {
        ExpectException(client =>
        {
            var request = new ExchangeRateRequest();
            client.Stream(request);
        }
        , exception =>
        {
            exception.Should().BeOfType<ArgumentException>();
            exception.As<ArgumentException>().ParamName.Should().Be("request");
            exception.Message.Should().ContainEquivalentOf("symbol");
        });
    }

    [Fact]
    public void StreamAsync_expects_currency()
    {
        ExpectException(client =>
        {
            var request = new ExchangeRateRequest { Symbol = "btc" };
            client.Stream(request);
        }
        , exception =>
        {
            exception.Should().BeOfType<ArgumentException>();
            exception.As<ArgumentException>().ParamName.Should().Be("request");
            exception.Message.Should().ContainEquivalentOf("currency");
        });
    }

    private static void ExpectException(Action<SpotExchangeRatesClient> operation, Action<Exception> assertions)
    {
        try
        {
            var sdkClient = Substitute.For<StreamAggregatesSpotExchangeRateServiceV1Client>();
            var client = new SpotExchangeRatesClient(sdkClient);
            operation(client);
        }
        catch (Exception x)
        {
            assertions(x);
        }
    }
}