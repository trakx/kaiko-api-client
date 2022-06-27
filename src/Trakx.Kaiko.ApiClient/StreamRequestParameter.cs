using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient
{
    /// <summary>
    /// Represents the input parameter (code and aggregate) required for streaming
    /// </summary>
    public class StreamRequestParameter
    {
        public StreamRequestParameter(string code, StreamRequestParameterAggregate aggregate)
        {
            Code = code;
            Aggregate = aggregate;
        }

        /// <summary>
        /// Instrument code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Aggregate value (1s, 1m, 1h)
        /// </summary>
        public StreamRequestParameterAggregate Aggregate { get; set; }
    }
}
