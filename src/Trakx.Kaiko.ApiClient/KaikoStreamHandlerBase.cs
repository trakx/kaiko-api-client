using System;
using System.Collections.Generic;
using System.Linq;

namespace Trakx.Kaiko.ApiClient 
{
    public class KaikoStreamHandlerBase: IObservable<IncomingCurrencyPair>
    {
        private readonly List<IObserver<IncomingCurrencyPair>> _observers = new List<IObserver<IncomingCurrencyPair>>();
        private IncomingCurrencyPair? _lastPair;

        public IDisposable Subscribe(IObserver<IncomingCurrencyPair> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);

                // Provide observer with existing data (ifnot null)
                if (_lastPair != null)
                {
                    observer.OnNext(_lastPair);
                }
            }

            //The provider's (KaikoStreamHandlerBase) Subscribe method returns an IDisposable implementation
            //that enables observers to stop receiving notifications
            return new Unsubscriber<IncomingCurrencyPair>(_observers, observer);
        }

        /// <summary>
        /// to be called when a new price is received
        /// </summary>
        /// <param name="symbol">Instrument code (for example btc-usd).</param>
        /// <param name="price">Price</param>
        public void UpdatePrice(string symbol, string price)
        {
            IncomingCurrencyPair item = new IncomingCurrencyPair(symbol, price);

            if (!item.Equals(_lastPair)) 
            {
                foreach (var observer in _observers)
                {
                    observer.OnNext(item);
                }

                _lastPair = item;
            }
        }
    }
}