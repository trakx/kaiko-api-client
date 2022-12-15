using System.Runtime.InteropServices;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Trakx.Kaiko.ApiClient.Stream.Tests
{
    public class ConfigTests
    {
        private readonly KaikoStreamConfiguration _config;

        public ConfigTests()
        {
            _config = ConfigurationHelper.GetConfigurationFromEnv<KaikoStreamConfiguration>();
        }

        [Fact]
        public void Config_should_have_channel_url()
        {
            _config.Should().NotBeNull();
            _config.ChannelUrl.Should().StartWith("https://");
        }

        [Fact]
        public void Config_should_have_api_key()
        {
            _config.Should().NotBeNull();
            _config.ApiKey.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Config_should_have_retry_policy()
        {
            _config.Should().NotBeNull();
            _config.RetryPolicy.Should().NotBeNull();
        }

        [Fact]
        public void Retry_policy_should_be_valid()
        {
            _config.Should().NotBeNull();
            
            var policy = _config.RetryPolicy;
            policy.Should().NotBeNull();
            
            policy.MaxAttempts.Should().BeGreaterThan(1);
            policy.BackoffMultiplier.Should().BeGreaterThan(0f);

            var zeroTime = TimeSpan.FromSeconds(0);
            policy.InitialBackoff.Should().BeGreaterThan(zeroTime);
            policy.MaxBackoff.Should().BeGreaterThan(zeroTime);
        }
    }
}