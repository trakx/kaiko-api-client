namespace Trakx.Kaiko.ApiClient.Stream.Tests;

[Collection(nameof(ApiTestCollection))]
public class ConfigTests
{
    private readonly KaikoStreamConfiguration _config;

    public ConfigTests(KaikoStreamFixture fixture)
    {
        _config = fixture.Config;
    }

    [Fact]
    public void Config_should_have_channel_url()
    {
        _config.Should().NotBeNull();
        _config.ChannelUrl.Should().NotBeNull();
        _config.ChannelUrl.OriginalString.Should().StartWith("https://");
    }

    [Fact]
    public void Config_should_have_api_key()
    {
        _config.Should().NotBeNull();
        _config.ApiKey.Should().NotBeNullOrWhiteSpace();
    }
}