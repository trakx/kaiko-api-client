using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient
{
    internal class Unsubscriber<IncomingCurrencyPair> : IDisposable
    {
        private List<IObserver<IncomingCurrencyPair>> _observers;
        private IObserver<IncomingCurrencyPair> _observer;

        internal Unsubscriber(List<IObserver<IncomingCurrencyPair>> observers, IObserver<IncomingCurrencyPair> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }                
        }
    }
}
