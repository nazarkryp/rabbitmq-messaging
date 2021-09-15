using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Clients;
using NKryp.Messaging.RabbitMq.Clients;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NKryp.Messaging.RabbitMq
{
    public class QueueClient : RabbitMqClientBase, IClient
    {
        #region Private Fields

        private readonly ILogger<QueueClient> _logger;
        private readonly string _queueName;

        #endregion

        #region Constructor

        public QueueClient(string uri, string password, string queueName, ILoggerFactory loggerFactory)
        : base(uri, password, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QueueClient>();

            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentNullException(nameof(queueName), "QueueName is missing");
            }

            _queueName = queueName;
        }

        #endregion

        #region Public Methods

        public Task SendAsync(IMessage message)
        {
            Channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            if (!message.UserProperties.TryGetValue("Key", out var key))
            {
                throw new ArgumentNullException(nameof(key), "Message key is missing");
            }

            var properties = Channel.CreateBasicProperties();
            properties.Type = key;
            properties.MessageId = Guid.NewGuid().ToString();
            properties.Headers = new Dictionary<string, object>
            {
                ["x-attempt-count"] = 0
            };

            Channel.BasicPublish(string.Empty, _queueName, properties, message.Body);

            return Task.CompletedTask;
        }

        public void Subscribe(Func<IMessage, Task> asyncFunc)
        {
            var basicConsumer = new EventingBasicConsumer(Channel);

            Channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            basicConsumer.Received += async (sender, e) =>
            {
                var message = new Message
                {
                    Body = e.Body.ToArray(),
                    UserProperties =
                    {
                        ["MessageId"] = e.BasicProperties.MessageId,
                        ["Key"] = e.BasicProperties.Type
                    }
                };
                
                try
                {
                    //_logger.LogInformation(" MessageId: {messageId}. Tag: {tag}.Redelivered: {redelivered}\n", e.BasicProperties.MessageId, e.DeliveryTag, e.Redelivered);

                    await asyncFunc(message);

                    Channel.BasicAck(e.DeliveryTag, false);
                }
                catch (Exception)
                {
                    if (e.Redelivered)
                    {
                        Channel.BasicAck(e.DeliveryTag, false);
                    }
                    else
                    {
                        Channel.BasicReject(e.DeliveryTag, true);
                    }
                }
            };

            Channel.BasicConsume(_queueName, false, basicConsumer);
        }

        #endregion
    }
}