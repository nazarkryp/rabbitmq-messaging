using System;

using Messaging.Clients;
using Messaging.Configuration;
using Messaging.Factories;
using Messaging.RabbitMq.Configuration;

using Microsoft.Extensions.Options;

namespace Messaging.RabbitMq.Factories
{
    public class RabbitMqClientFactory<TConfiguration> : IClientFactory<TConfiguration> where TConfiguration : class
    {
        private readonly IOptions<TConfiguration> _options;
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;

        public RabbitMqClientFactory(
            IOptions<TConfiguration> options,
            IRabbitMqConfiguration rabbitMqConfiguration)
        {
            _options = options;
            _rabbitMqConfiguration = rabbitMqConfiguration;
        }

        public IClient Create()
        {
            if (_options.Value is IQueueConfiguration queueConfiguration)
            {
                return new QueueClient(_rabbitMqConfiguration.AmqpUrl, _rabbitMqConfiguration.Password, queueConfiguration.QueueName);
            }

            if (_options.Value is ITopicConfiguration topicConfiguration)
            {
                return new TopicClient(_rabbitMqConfiguration.AmqpUrl, _rabbitMqConfiguration.Password, topicConfiguration.TopicName);
            }

            throw new NotSupportedException($"The {_options.Value.GetType()} not supported.");
        }
    }
}
