using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Kaiko.ApiClient.Stream
{
    internal class ChannelProvider
    {
        /// <summary>
        /// Creates the Channel object required for streaming
        /// </summary>
        /// <param name="channelTarget">target of channel</param>
        /// <param name="apiKey">api key</param>
        /// <returns></returns>
        public static Channel Create(string channelTarget, string apiKey)
        {
            Channel channel = new Channel(channelTarget, CreateAuthenticatedChannel(apiKey));

            return channel;
        }

        /// <summary>
        /// builds the authentication object
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private static ChannelCredentials CreateAuthenticatedChannel(string apiKey)
        {
            var interceptor = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(apiKey))
                {
                    metadata.Add("Authorization", $"Bearer {apiKey}");
                }
                return Task.CompletedTask;
            });

            return ChannelCredentials.Create(new SslCredentials(), interceptor);
        }
    }
}
