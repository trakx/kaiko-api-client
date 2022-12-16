using static KaikoSdk.StreamAggregatesDirectExchangeRateServiceV1;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class DirectExchangeRatesClientTests
{
    [Fact]
    public void StreamAsync_expects_request()
    {
        ExpectException(client =>
        {
            client.StreamAsync(null);
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
            client.StreamAsync(request);
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
            client.StreamAsync(request);
        }
        , exception =>
        {
            exception.Should().BeOfType<ArgumentException>();
            exception.As<ArgumentException>().ParamName.Should().Be("request");
            exception.Message.Should().ContainEquivalentOf("currency");
        });
    }

    private static void ExpectException(Action<DirectExchangeRatesClient> operation, Action<Exception> assertions)
    {
        try
        {
            var sdkClient = Substitute.For<StreamAggregatesDirectExchangeRateServiceV1Client>();
            var client = new DirectExchangeRatesClient(sdkClient);
            operation(client);
        }
        catch (Exception x)
        {
            assertions(x);
        }
    }
}