using System;
using System.Collections.Generic;

using Messaging.Clients;
using Messaging.Configuration;
using Messaging.Factories;
using Messaging.Serialization;

namespace Messaging.Services
{
    public class MessageListenerService
    {
        private readonly IEnumerable<IQueueConfiguration> _queueConfigurations;
        private readonly IEnumerable<ITopicConfiguration> _topicConfigurations;
        private readonly IQueueClientFactory _queueClientFactory;
        private readonly ITopicClientFactory _topicClientFactory;
        private readonly IMessageHandlerFactory _handlerFactory;
        private readonly IMessageSerializer _messageSerializer;

        private readonly IList<IClient> _clients;

        public MessageListenerService(
            IEnumerable<IQueueConfiguration> queueConfigurations,
            IEnumerable<ITopicConfiguration> topicConfigurations,
            IQueueClientFactory queueClientFactory,
            ITopicClientFactory topicClientFactory,
            IMessageHandlerFactory handlerFactory,
            IMessageSerializer messageSerializer)
        {
            _queueConfigurations = queueConfigurations;
            _topicConfigurations = topicConfigurations;
            _queueClientFactory = queueClientFactory;
            _topicClientFactory = topicClientFactory;
            _handlerFactory = handlerFactory;
            _messageSerializer = messageSerializer;
            _clients = new List<IClient>();
        }

        public void StartListeners()
        {
            foreach (var queueConfiguration in _queueConfigurations)
            {
                var queueClient = _queueClientFactory.Create(queueConfiguration.QueueName);
                _clients.Add(queueClient);

                queueClient.Subscribe(MessageReceived);
            }

            foreach (var topicConfiguration in _topicConfigurations)
            {
                var topicClient = _topicClientFactory.Create(topicConfiguration.TopicName);
                _clients.Add(topicClient);

                topicClient.Subscribe(MessageReceived);
            }
        }

        public void StopListeners()
        {
            foreach (var client in _clients)
            {
                client.Dispose();
            }
        }

        private async void MessageReceived(IMessage message)
        {
            if (!message.UserProperties.TryGetValue("Key", out var key))
            {
                throw new Exception("Key missing");
            }

            var type = Type.GetType(key);
            var handler = _handlerFactory.Create(type);
            var command = _messageSerializer.Deserialize(message, type);

            try
            {
                await handler.HandleAsync(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
