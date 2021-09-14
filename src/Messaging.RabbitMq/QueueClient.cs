using System;
using System.Threading.Tasks;

using Messaging.Clients;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.RabbitMq
{
    public class QueueClient : IClient
    {
        private readonly string _queueName;
        private readonly IModel _channel;

        public QueueClient(string amqpUrl, string password, string queueName)
        {
            IConnectionFactory connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(amqpUrl),
                Password = password
            };

            _channel = connectionFactory.CreateConnection().CreateModel();
            _queueName = queueName;
        }

        public Task SendAsync(IMessage message)
        {
            var command = (Message)message;

            _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            if (!command.UserProperties.TryGetValue("Key", out var key))
            {
                throw new ArgumentNullException(nameof(key), "Message key is missing");
            }

            var properties = _channel.CreateBasicProperties();
            properties.Type = key;

            _channel.BasicPublish(exchange: string.Empty, _queueName, properties, command.Body);

            return Task.CompletedTask;
        }

        public void Subscribe(Action<IMessage> action)
        {
            var basicConsumer = new EventingBasicConsumer(_channel);

            _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            basicConsumer.Received += (sender, e) =>
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

                action(message);
            };

            _channel.BasicConsume(_queueName, true, basicConsumer);
        }

        public void Dispose()
        {
            _channel.Close();
        }
    }
}