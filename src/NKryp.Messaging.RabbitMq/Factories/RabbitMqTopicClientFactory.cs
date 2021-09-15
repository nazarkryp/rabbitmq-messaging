using System;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Clients;
using NKryp.Messaging.Factories;
using NKryp.Messaging.RabbitMq.Configuration;

namespace NKryp.Messaging.RabbitMq.Factories
{
    public class RabbitMqTopicClientFactory : ITopicClientFactory, IDisposable
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        private TopicClient _topicClient;

        public RabbitMqTopicClientFactory(IRabbitMqConfiguration rabbitMqConfiguration, ILoggerFactory loggerFactory)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _loggerFactory = loggerFactory;
        }

        public IClient Create(string topicName, bool createNew)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentNullException(nameof(topicName), "TopicName is required");
            }

            if (createNew)
            {
                return new TopicClient(_rabbitMqConfiguration.Uri, _rabbitMqConfiguration.Password, topicName, _loggerFactory);
            }

            return _topicClient ??= new TopicClient(_rabbitMqConfiguration.Uri, _rabbitMqConfiguration.Password, topicName, _loggerFactory);
        }

        public void Dispose()
        {
            _topicClient?.Dispose();
        }
    }
}
