using System;
using System.Threading.Tasks;

using NKryp.Messaging.Clients;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NKryp.Messaging.RabbitMq
{
    public class QueueClient : IClient
    {
        private readonly string _queueName;
        private readonly IConnectionFactory _connectionFactory;

        private IConnection _connection;
        private IModel _channel;

        public QueueClient(string amqpUrl, string password, string queueName)
        {
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(amqpUrl),
                Password = password
            };

            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentNullException(nameof(queueName), "Queue name cannot be empty");
            }

            _queueName = queueName;
        }

        #region Private Properties

        private IConnection Connection => _connection ??= _connectionFactory.CreateConnection();

        private IModel Channel => _channel ??= Connection.CreateModel();

        #endregion

        #region Public Methods

        public Task SendAsync(IMessage message)
        {
            var command = (Message)message;

            Channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            if (!command.UserProperties.TryGetValue("Key", out var key))
            {
                throw new ArgumentNullException(nameof(key), "Message key is missing");
            }

            var properties = Channel.CreateBasicProperties();
            properties.Type = key;

            Channel.BasicPublish(exchange: string.Empty, _queueName, properties, command.Body);

            return Task.CompletedTask;
        }

        public void Subscribe(Func<IMessage, Task> asyncFunc)
        {
            var basicConsumer = new EventingBasicConsumer(Channel);

            Channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

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
                    await asyncFunc(message);
                }
                catch (Exception)
                {
                    // ignored
                }
            };

            Channel.BasicConsume(_queueName, true, basicConsumer);
        }

        public void Dispose()
        {
            Channel.Close();
        }

        #endregion
    }
}