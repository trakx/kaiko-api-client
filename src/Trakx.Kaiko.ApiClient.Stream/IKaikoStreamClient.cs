using Trakx.Kaiko.ApiClient.Stream.Models;

namespace Trakx.Kaiko.ApiClient.Stream
{
    public interface IKaikoStreamClient
    {
        IAsyncEnumerable<SpotExchangeRateResponse> StreamSpotExchangeRates(
            string symbol,
            string currency = "usd",
            CancellationToken? cancellationToken = null);
    }
}