using System;

using NKryp.Messaging.Clients;
using NKryp.Messaging.Factories;
using NKryp.Messaging.RabbitMq.Configuration;

namespace NKryp.Messaging.RabbitMq.Factories
{
    public class RabbitMqTopicClientFactory : ITopicClientFactory, IDisposable
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;

        private TopicClient _topicClient;

        public RabbitMqTopicClientFactory(IRabbitMqConfiguration rabbitMqConfiguration)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
        }

        public IClient Create(string topicName)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentNullException(nameof(topicName), "TopicName is required");
            }

            return _topicClient ??= new TopicClient(_rabbitMqConfiguration.AmqpUrl, _rabbitMqConfiguration.Password, topicName);
        }

        public void Dispose()
        {
            _topicClient?.Dispose();
        }
    }
}
