using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient.Stream.Models
{
    public class SpotExchangeRateResponse
    {

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("code")]
        public DateTime Timestamp { get; set; }

    }
}
