using System;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Clients;
using NKryp.Messaging.Factories;
using NKryp.Messaging.RabbitMq.Configuration;

namespace NKryp.Messaging.RabbitMq.Factories
{
    public class RabbitMqQueueClientFactory : IQueueClientFactory, IDisposable
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        private QueueClient _queueClient;

        public RabbitMqQueueClientFactory(IRabbitMqConfiguration rabbitMqConfiguration, ILoggerFactory loggerFactory)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _loggerFactory = loggerFactory;
        }

        public IClient Create(string queueName, bool createNew = false)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentNullException(nameof(queueName), "QueueName is required");
            }

            if (createNew)
            {
                return new QueueClient(_rabbitMqConfiguration.Uri, _rabbitMqConfiguration.Password, queueName, _loggerFactory);
            }

            return _queueClient ??= new QueueClient(_rabbitMqConfiguration.Uri, _rabbitMqConfiguration.Password, queueName, _loggerFactory);
        }

        public void Dispose()
        {
            _queueClient?.Dispose();
        }
    }
}