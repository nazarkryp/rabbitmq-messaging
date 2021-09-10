using System;
using System.Threading.Tasks;

using Messaging.Clients;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.RabbitMq
{
    public class TopicClient : IClient
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly string _topicName;

        private IConnection _connection;
        private IModel _channel;

        public TopicClient(string amqpUrl, string password, string topicName)
        {
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(amqpUrl),
                Password = password
            };

            _topicName = topicName;
        }

        #region Private Properties

        private IConnection Connection => _connection ??= _connectionFactory.CreateConnection();

        private IModel Chanel => _channel ??= Connection.CreateModel();

        #endregion

        #region Public Methods

        public Task SendAsync(IMessage message)
        {
            var command = (Message)message;

            Chanel.ExchangeDeclare(_topicName, ExchangeType.Topic, durable: true);

            if (!command.UserProperties.TryGetValue("Key", out var key))
            {
                throw new ArgumentNullException(nameof(key), "Message key is missing");
            }

            var properties = Chanel.CreateBasicProperties();
            properties.Type = key;

            Chanel.BasicPublish(
                exchange: _topicName,
                routingKey: $"{_topicName}.ALL.subscription",
                basicProperties: properties,
                body: command.Body);

            return Task.CompletedTask;
        }

        public void Subscribe(Action<IMessage> action)
        {
            var queueName = Chanel.QueueDeclare($"{_topicName}.subscription.queue", durable: true, autoDelete: false, exclusive: false).QueueName;

            Chanel.ExchangeDeclare(_topicName, ExchangeType.Topic, durable: true);
            Chanel.QueueBind(
                queue: queueName,
                exchange: _topicName,
                routingKey: $"{_topicName}.*.subscription");

            var basicConsumer = new EventingBasicConsumer(Chanel);

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

            Chanel.BasicConsume(queueName, true, basicConsumer);
        }

        public void Dispose()
        {
            Connection.Dispose();
            Chanel.Dispose();
        }

        #endregion
    }
}
