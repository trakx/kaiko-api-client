using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient.Stream
{
    public interface IKaikoStreamHandler : IDisposable
    {
        void UpdatePrice(string symbol, string price);

        void Start(); 

        void Stop();
    }
}
