using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient.Stream;

/// <summary>
/// Time intervals supported by the API when aggregating prices.
/// </summary>
public enum AggregateInterval
{
    /// <summary>
    /// 1s
    /// </summary>
    OneSecond = 1,

    /// <summary>
    /// 1m
    /// </summary>
    OneMinute = 60,

    /// <summary>
    /// 1h
    /// </summary>
    OneHour = 360,
}

/// <summary>
/// Helper functions to work with <see cref="AggregateInterval"/>
/// </summary>
internal static class AggregateIntervalExtensions
{
    /// <summary>
    /// Convert <see cref="AggregateInterval"/> into the expected parameter string.
    /// </summary>
    internal static string ToApiParameter(this AggregateInterval interval)
    {
        return interval switch
        {
            AggregateInterval.OneMinute => "1m",
            AggregateInterval.OneHour => "1h",
            _ => "1s",
        };
    }
}
