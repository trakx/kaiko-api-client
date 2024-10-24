//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Trakx.Common.ApiClient;
using Trakx.Common.ApiClient.Exceptions;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"

namespace Trakx.Kaiko.ApiClient
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial interface IAggregatesClient
    {

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <remarks>
        /// This endpoint retrieves the OHLCV history for an instrument on an exchange.
        /// <br/>The interval parameter is suffixed with s, m, h or d to specify seconds, minutes, hours or days, respectively.
        /// <br/>By making use of the sort parameter, data can be returned in ascending asc or descending desc order.
        /// </remarks>
        /// <param name="commodity">The data commodity.</param>
        /// <param name="data_version">The data version</param>
        /// <param name="exchange">The code for the desired exchange.</param>
        /// <param name="instrument_class">The class of the instrument.</param>
        /// <param name="instrument">The code of the instrument.</param>
        /// <param name="interval">The interval period.</param>
        /// <returns>Aggregate OHLCV Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<Response<AggregateOhlcvResponse>> GetAggregateOhlcvAsync(Commodity commodity, DataVersion data_version, string exchange, string instrument_class, string instrument, Interval? interval = null, System.DateTimeOffset? start_time = null, System.DateTimeOffset? end_time = null, int? page_size = null, SortOrder? sort = null, string continuation_token = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <remarks>
        /// This endpoint retrieves aggregated VWAP (volume-weighted average price) history for an instrument on an exchange.
        /// <br/>The interval parameter is suffixed with s, m, h or d to specify seconds, minutes, hours or days, respectively.
        /// <br/>By making use of the sort parameter, data can be returned in ascending asc or descending desc (default) order.
        /// </remarks>
        /// <param name="commodity">The data commodity.</param>
        /// <param name="data_version">The data version</param>
        /// <param name="exchange">The code for the desired exchange.</param>
        /// <param name="instrument_class">The class of the instrument.</param>
        /// <param name="instrument">The code of the instrument.</param>
        /// <param name="interval">The interval period.</param>
        /// <returns>Aggregate VWAP Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<Response<AggregateVwapResponse>> GetAggregateVwapAsync(Commodity commodity, DataVersion data_version, string exchange, string instrument_class, string instrument, Interval? interval = null, System.DateTimeOffset? start_time = null, System.DateTimeOffset? end_time = null, int? page_size = null, SortOrder? sort = null, string continuation_token = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <remarks>
        /// This endpoint retrieves the trade count, OHLCV and VWAP history for an instrument on an exchange.
        /// <br/>The interval parameter is suffixed with s, m, h or d to specify seconds, minutes, hours or days, respectively.
        /// <br/>By making use of the sort parameter, data can be returned in ascending asc (default) or descending desc order.
        /// </remarks>
        /// <param name="commodity">The data commodity.</param>
        /// <param name="data_version">The data version</param>
        /// <param name="exchange">The code for the desired exchange.</param>
        /// <param name="instrument_class">The class of the instrument.</param>
        /// <param name="instrument">The code of the instrument.</param>
        /// <param name="interval">The interval period.</param>
        /// <returns>Aggregate Count OHLC Volume VWAP Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<Response<AggregateCountOhlcvVwapResponse>> GetAggregateCountOhlcvVwapAsync(Commodity commodity, DataVersion data_version, string exchange, string instrument_class, string instrument, Interval? interval = null, System.DateTimeOffset? start_time = null, System.DateTimeOffset? end_time = null, int? page_size = null, SortOrder? sort = null, string continuation_token = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial interface ITradesClient
    {

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <remarks>
        /// This endpoint retrieves trades for an instrument on a specific exchange.
        /// <br/>Trades are sorted by time, in a descendingly order unless it is specified otherwise.
        /// <br/>Note that taker_side_sell can be null in the cases where this information was not available at collection.
        /// </remarks>
        /// <param name="commodity">The data commodity.</param>
        /// <param name="data_version">The data version</param>
        /// <param name="exchange">The code for the desired exchange.</param>
        /// <param name="instrument_class">The class of the instrument.</param>
        /// <param name="instrument">The code of the instrument.</param>
        /// <returns>Trades Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<Response<TradesResponse>> GetTradesAsync(Commodity commodity, DataVersion data_version, string exchange, string instrument_class, string instrument, System.DateTimeOffset? start_time = null, System.DateTimeOffset? end_time = null, int? page_size = null, SortOrder? sort = null, string continuation_token = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }

    /// <summary>
    /// The chosen commodity. For the trades and order_book_snapshots commodities the latest version is currently v1.
    /// <br/>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum Commodity
    {

        [System.Runtime.Serialization.EnumMember(Value = @"trades")]
        Trades = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"instrument_metrics")]
        Instrument_metrics = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"order_book_snapshots")]
        Order_book_snapshots = 2,

    }

    /// <summary>
    /// The data version of the commodity. For the trades and order_book_snapshots commodities the latest version is currently v1.
    /// <br/>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum DataVersion
    {

        [System.Runtime.Serialization.EnumMember(Value = @"v1")]
        V1 = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"v2")]
        V2 = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"latest")]
        Latest = 2,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Access
    {
        [Newtonsoft.Json.JsonProperty("access_range", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DateRange Access_range { get; set; }

        [Newtonsoft.Json.JsonProperty("data_range", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DateRange Data_range { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DateRange
    {
        [Newtonsoft.Json.JsonProperty("start_timestamp", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long? Start_timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty("end_timestamp", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long? End_timestamp { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// Any arbitrary value between one second and one day can be used as an interval.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum Interval
    {

        [System.Runtime.Serialization.EnumMember(Value = @"1s")]
        _1s = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"1m")]
        _1m = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"5m")]
        _5m = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"1h")]
        _1h = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"4h")]
        _4h = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"1d")]
        _1d = 5,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum Aggregation
    {

        [System.Runtime.Serialization.EnumMember(Value = @"ohlcv")]
        Ohlcv = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"vwap")]
        Vwap = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"count_ohlcv_vwap")]
        Count_ohlcv_vwap = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"full")]
        Full = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"depth")]
        Depth = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"slippage")]
        Slippage = 5,

    }

    /// <summary>
    /// DateTime order in which the data is returned.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum SortOrder
    {

        [System.Runtime.Serialization.EnumMember(Value = @"asc")]
        Asc = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"desc")]
        Desc = 1,

    }

    /// <summary>
    /// All API responses are in JSON format.
    /// <br/>A result field, with a value of success or error is returned with each request.
    /// <br/>In the event of an error, a message field will provide an error message.
    /// <br/>An access object is also echoed back. It contains two ranges of timestamps
    /// <br/>  access_range - The time range for which the Client has access to the API
    /// <br/>  data_range - The time range of data the Client is authorized to access
    /// <br/>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ApiResponse
    {
        /// <summary>
        /// Time ranges of accesses.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("access", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Access Access { get; set; }

        [Newtonsoft.Json.JsonProperty("result", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ApiResponseResult? Result { get; set; }

        /// <summary>
        /// The current time at our endpoint.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("time", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset Time { get; set; }

        /// <summary>
        /// The current time at our endpoint.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long Timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty("continuation_token", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Continuation_token { get; set; }

        [Newtonsoft.Json.JsonProperty("next_url", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Next_url { get; set; }

        /// <summary>
        /// All handled query parameters echoed back.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("query", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public MarketQuery Query { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class MarketQuery
    {
        [Newtonsoft.Json.JsonProperty("commodity", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Commodity? Commodity { get; set; }

        [Newtonsoft.Json.JsonProperty("data_version", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public DataVersion? Data_version { get; set; }

        [Newtonsoft.Json.JsonProperty("exchange", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Exchange { get; set; }

        [Newtonsoft.Json.JsonProperty("instrument_class", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Instrument_class { get; set; }

        [Newtonsoft.Json.JsonProperty("instrument", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Instrument { get; set; }

        [Newtonsoft.Json.JsonProperty("page_size", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1, 10000)]
        public int Page_size { get; set; }

        [Newtonsoft.Json.JsonProperty("request_time", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset Request_time { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AggregateOhlcvResponse : ApiResponse
    {
        /// <summary>
        /// Response result data.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<AggregateOhlcvData> Data { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AggregateOhlcvData
    {
        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long Timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty("open", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Open { get; set; }

        [Newtonsoft.Json.JsonProperty("high", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? High { get; set; }

        [Newtonsoft.Json.JsonProperty("low", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Low { get; set; }

        [Newtonsoft.Json.JsonProperty("close", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Close { get; set; }

        [Newtonsoft.Json.JsonProperty("volume", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Volume { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AggregateVwapResponse : ApiResponse
    {
        /// <summary>
        /// Response result data.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<AggregateVwapData> Data { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AggregateVwapData
    {
        /// <summary>
        /// Timestamp at which the interval begins.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long Timestamp { get; set; }

        /// <summary>
        /// Volume-weighted average price. null when no trades reported.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("price", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Price { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AggregateCountOhlcvVwapResponse : ApiResponse
    {
        /// <summary>
        /// Response result data.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<AggregateCountOhlcvVwapData> Data { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AggregateCountOhlcvVwapData
    {
        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long Timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty("count", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [Newtonsoft.Json.JsonProperty("open", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Open { get; set; }

        [Newtonsoft.Json.JsonProperty("high", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? High { get; set; }

        [Newtonsoft.Json.JsonProperty("low", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Low { get; set; }

        [Newtonsoft.Json.JsonProperty("close", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Close { get; set; }

        [Newtonsoft.Json.JsonProperty("price", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Price { get; set; }

        [Newtonsoft.Json.JsonProperty("volume", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Volume { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TradesResponse : ApiResponse
    {
        /// <summary>
        /// Response result data.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<TradesResponseData> Data { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TradesResponseData
    {
        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long Timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty("trade_id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Trade_id { get; set; }

        [Newtonsoft.Json.JsonProperty("price", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Price { get; set; }

        [Newtonsoft.Json.JsonProperty("amount", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Amount { get; set; }

        [Newtonsoft.Json.JsonProperty("taker_side_sell", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool? Taker_side_sell { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.2.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum ApiResponseResult
    {

        [System.Runtime.Serialization.EnumMember(Value = @"success")]
        Success = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"error")]
        Error = 1,

    }


}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625