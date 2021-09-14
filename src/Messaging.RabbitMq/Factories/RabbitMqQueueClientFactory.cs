using System;

using Messaging.Clients;
using Messaging.Factories;
using Messaging.RabbitMq.Configuration;

namespace Messaging.RabbitMq.Factories
{
    public class RabbitMqQueueClientFactory : IQueueClientFactory
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;

        public RabbitMqQueueClientFactory(IRabbitMqConfiguration rabbitMqConfiguration)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
        }

        public IClient Create(string queueName)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentNullException(nameof(queueName), "Queue name is required");
            }
            
            return new QueueClient(_rabbitMqConfiguration.AmqpUrl, _rabbitMqConfiguration.Password, queueName);
        }
    }
}