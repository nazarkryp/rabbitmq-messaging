using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Clients;
using NKryp.Messaging.Factories;
using NKryp.Messaging.Handlers;
using NKryp.Messaging.Infrastructure.Providers;
using NKryp.Messaging.Serialization;

namespace NKryp.Messaging.Services
{
    public interface IMessageListenerService
    {
        void StartListeners();

        void StopListeners();
    }

    public class MessageListenerService : IMessageListenerService
    {
        private readonly IEnumerable<IMessageHandler> _messageHandlers;
        private readonly IQueueClientFactory _queueClientFactory;
        private readonly ITopicClientFactory _topicClientFactory;
        private readonly IMessageHandlerFactory _handlerFactory;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IList<IClient> _clients;
        private IMessageTypeProvider _messageTypeProvider;
        private readonly ILogger<MessageListenerService> _logger;

        public MessageListenerService(
            IEnumerable<IMessageHandler> messageHandlers,
            IQueueClientFactory queueClientFactory,
            ITopicClientFactory topicClientFactory,
            IMessageHandlerFactory handlerFactory,
            IMessageSerializer messageSerializer,
            ILogger<MessageListenerService> logger)
        {
            _messageHandlers = messageHandlers;
            _queueClientFactory = queueClientFactory;
            _topicClientFactory = topicClientFactory;
            _handlerFactory = handlerFactory;
            _messageSerializer = messageSerializer;
            _logger = logger;
            _clients = new List<IClient>();
        }

        public void StartListeners()
        {
            var handledMessages = new List<Type>();

            foreach (var messageHandler in _messageHandlers)
            {
                var messageHandlerInterfaces = messageHandler.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMessageHandler<>));
                var genericArgumentTypes = messageHandlerInterfaces.Select(x => x.GetGenericArguments()[0]).ToList();
                handledMessages.AddRange(genericArgumentTypes);

                foreach (var genericArgumentType in genericArgumentTypes)
                {
                    var queueName = QueueNameProvider.GetQueueName(genericArgumentType);
                    var topicName = TopicNameProvider.GetTopicName(genericArgumentType);

                    if (!string.IsNullOrEmpty(queueName))
                    {
                        var client = _queueClientFactory.Create(queueName, true);
                        _clients.Add(client);
                    }

                    if (!string.IsNullOrEmpty(topicName))
                    {
                        var client = _topicClientFactory.Create(topicName);
                        _clients.Add(client);
                    }
                }
            }

            _messageTypeProvider = new MessageTypeAttributeProvider(handledMessages);

            foreach (var client in _clients)
            {
                client.Subscribe(MessageReceived);
            }
        }

        public void StopListeners()
        {
            foreach (var client in _clients)
            {
                client.Dispose();
            }
        }

        private async Task MessageReceived(IMessage message)
        {
            if (!message.UserProperties.TryGetValue("Key", out var key))
            {
                throw new ArgumentException("Message key is missing", nameof(key));
            }

            var messageType = _messageTypeProvider.GetMessageType(key);
            var handler = _handlerFactory.Create(messageType);
            var command = _messageSerializer.Deserialize(message, messageType);

            try
            {
                await handler.HandleAsync(command);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error processing message");

                throw;
            }
        }
    }
}
