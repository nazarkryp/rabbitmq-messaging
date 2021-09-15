using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Clients;
using NKryp.Messaging.RabbitMq.Clients;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NKryp.Messaging.RabbitMq
{
    public class TopicClient : RabbitMqClientBase, IClient
    {
        private readonly ILogger<TopicClient> _logger;
        private readonly string _topicName;

        public TopicClient(string uri, string password, string topicName, ILoggerFactory loggerFactory)
        : base(uri, password, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TopicClient>();

            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentNullException(nameof(topicName), "Topic name cannot be empty");
            }

            _topicName = topicName;
        }

        #region Public Methods

        public Task SendAsync(IMessage message)
        {
            var command = (Message)message;

            Channel.ExchangeDeclare(_topicName, ExchangeType.Topic, durable: true);

            if (!command.UserProperties.TryGetValue("Key", out var key))
            {
                throw new ArgumentNullException(nameof(key), "Message key is missing");
            }

            var properties = Channel.CreateBasicProperties();
            properties.Type = key;

            Channel.BasicPublish(
                exchange: _topicName,
                routingKey: $"{_topicName}.ALL.subscription",
                basicProperties: properties,
                body: command.Body);

            return Task.CompletedTask;
        }

        public void Subscribe(Func<IMessage, Task> asyncFunc)
        {
            var queueName = Channel.QueueDeclare($"{_topicName}.subscription.queue", durable: true, autoDelete: false, exclusive: false).QueueName;

            Channel.ExchangeDeclare(_topicName, ExchangeType.Topic, durable: true);
            Channel.QueueBind(
                queue: queueName,
                exchange: _topicName,
                routingKey: $"{_topicName}.*.subscription");

            var basicConsumer = new EventingBasicConsumer(Channel);

            basicConsumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                
                var message = new Message
                {
                    Body = body,
                    UserProperties =
                    {
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

            Channel.BasicConsume(queueName, true, basicConsumer);
        }

        #endregion
    }
}
