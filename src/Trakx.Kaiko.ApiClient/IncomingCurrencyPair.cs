using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient
{
    public class IncomingCurrencyPair
    {
        internal IncomingCurrencyPair(string symbol, string price)
        {
            this.Symbol = symbol;
            this.Price = price;
        }

        /// <summary>
        /// Instrument code (for example btc-usd).
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// checks if object instances of IncomingCurrencyPair are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>boolean</returns>
        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                //check equality of symbols /  prices
                IncomingCurrencyPair pair = (IncomingCurrencyPair) obj;
                return (Symbol == pair.Symbol) && (Price == pair.Price);
            }
        }
    }
}
