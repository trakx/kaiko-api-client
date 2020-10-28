using System.Collections.Generic;

namespace Trakx.Kaiko.ApiClient
{
    public class KaikoApiConfiguration
    {
#nullable disable
        public string ApiKey { get; set; }
        public List<string> FavouriteExchanges { get; set; }
#nullable restore
    }
}
