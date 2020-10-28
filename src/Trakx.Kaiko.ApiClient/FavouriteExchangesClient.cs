using System.Collections.Generic;

namespace Trakx.Kaiko.ApiClient
{
    internal abstract class FavouriteExchangesClient : IFavouriteExchangesClient
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
                    "stmp", "btrx", "polo", "krkn", "bfnx", "cbse", 
                    "itbi", "gmni", "bnce", "bfly", "cflr", "huob"
                }.AsReadOnly();

            Top12ExchangeIdsAsCsv = string.Join(",", Top12ExchangeIds);
        }

        public KaikoApiConfiguration ApiConfiguration { get; protected set; }
    }
}