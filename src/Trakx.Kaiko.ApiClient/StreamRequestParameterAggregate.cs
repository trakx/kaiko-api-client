using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient
{

    public class StreamRequestParameterAggregate
    {
        private StreamRequestParameterAggregate(string value) { Value = value; }

        public string Value { get; private set; }

        /// <summary>
        /// returns aggregate value (1s)
        /// </summary>
        public static StreamRequestParameterAggregate OneSecond { get { return new StreamRequestParameterAggregate("1s"); } }

        /// <summary>
        /// returns aggregate value (1m)
        /// </summary>
        public static StreamRequestParameterAggregate OneMinute { get { return new StreamRequestParameterAggregate("1m"); } }

        /// <summary>
        /// returns aggregate value (1h)
        /// </summary>
        public static StreamRequestParameterAggregate OneHour { get { return new StreamRequestParameterAggregate("1h"); } }
    }
}
