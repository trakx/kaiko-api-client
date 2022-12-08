using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient;

public interface ITradabilityChecker
{
    Task<IList<string>> GetTradableAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default);
}

public class TradabilityChecker : ITradabilityChecker
{
    private readonly IMarketDataClient _marketDataClient;

    public TradabilityChecker(IMarketDataClient marketDataClient)
    {
        _marketDataClient = marketDataClient;
    }

    public async Task<IList<string>> GetTradableAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default)
    {
        var exchanges = ((IFavouriteExchangesClient)_marketDataClient).Top12ExchangeIds;
        var untradableSymbols = new HashSet<string>(symbols);
        foreach (var exchangeName in exchanges)
        {
            if (Enum.TryParse(typeof(Exchange), exchangeName, true, out var exchange) && exchange != null)
            {
                var tickers = await _marketDataClient.GetTickerAsync((Exchange)exchange, cancellationToken);
                foreach (var ticker in tickers.Result)
                    untradableSymbols.Remove(ticker.Symbol.ToLower());

                if (untradableSymbols.Count == 0)
                    break;
            }
        }
        return symbols.Except(untradableSymbols).ToList();
    }
}