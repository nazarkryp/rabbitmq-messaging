using System;

using NKryp.Messaging.Clients;
using NKryp.Messaging.Factories;
using NKryp.Messaging.RabbitMq.Configuration;

namespace NKryp.Messaging.RabbitMq.Factories
{
    public class RabbitMqQueueClientFactory : IQueueClientFactory, IDisposable
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;

        private QueueClient _queueClient;

        public RabbitMqQueueClientFactory(IRabbitMqConfiguration rabbitMqConfiguration)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
        }

        public IClient Create(string queueName, bool createNew = false)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentNullException(nameof(queueName), "QueueName is required");
            }

            if (createNew)
            {
                return new QueueClient(_rabbitMqConfiguration.AmqpUrl, _rabbitMqConfiguration.Password, queueName);
            }
            
            return _queueClient ??= new QueueClient(_rabbitMqConfiguration.AmqpUrl, _rabbitMqConfiguration.Password, queueName);
        }

        public void Dispose()
        {
            _queueClient?.Dispose();
        }
    }
}