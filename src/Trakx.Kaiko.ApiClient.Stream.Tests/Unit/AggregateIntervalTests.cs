namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class AggregateIntervalTests
{
    [Theory]
    [InlineData(AggregateInterval.OneSecond, "1s")]
    [InlineData(AggregateInterval.OneMinute, "1m")]
    [InlineData(AggregateInterval.OneHour, "1h")]
    public void Mapping_works(AggregateInterval interval, string expected)
    {
        var mapped = AggregateIntervalExtensions.ToApiParameter(interval);
        mapped.Should().Be(expected);
    }
}