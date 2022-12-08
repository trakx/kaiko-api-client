using System.Collections.Generic;

namespace Trakx.Kaiko.ApiClient;

public abstract class FavouriteExchangesClient : IFavouriteExchangesClient
{
    public IReadOnlyList<string> Top12ExchangeIds { get; }
    public string Top12ExchangeIdsAsCsv { get; }

    protected FavouriteExchangesClient(ClientConfigurator clientConfigurator)
    {
        ApiConfiguration = clientConfigurator.ApiConfiguration;
        Top12ExchangeIds = ApiConfiguration.FavouriteExchanges?.Count > 0
            ? ApiConfiguration.FavouriteExchanges!.AsReadOnly()
            : new List<string>
            {
                "binance", "binanceUs", "ftx", "ftxus", "coinbasePro", "kraken", "kucoin", "huobiGlobal", "okex", "gemini", "gateio"
            }.AsReadOnly();

        Top12ExchangeIdsAsCsv = string.Join(",", Top12ExchangeIds);
    }

    public KaikoApiConfiguration ApiConfiguration { get; init; }
}
