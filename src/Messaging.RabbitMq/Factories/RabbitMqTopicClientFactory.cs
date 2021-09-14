using System;

using Messaging.Clients;
using Messaging.Factories;
using Messaging.RabbitMq.Configuration;

namespace Messaging.RabbitMq.Factories
{
    public class RabbitMqTopicClientFactory : ITopicClientFactory
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;

        public RabbitMqTopicClientFactory(IRabbitMqConfiguration rabbitMqConfiguration)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
        }

        public IClient Create(string topicName)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentNullException(nameof(topicName), "Topic name is required");
            }

            return new TopicClient(_rabbitMqConfiguration.AmqpUrl, _rabbitMqConfiguration.Password, topicName);
        }
    }
}
