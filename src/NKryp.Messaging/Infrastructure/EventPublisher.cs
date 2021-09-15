using System.Threading.Tasks;

using NKryp.Messaging.Factories;
using NKryp.Messaging.Infrastructure.Providers;
using NKryp.Messaging.Serialization;

namespace NKryp.Messaging.Infrastructure
{
    public interface IEventPublisher
    {
        Task PublishAsync(object @event);
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly ITopicClientFactory _topicClientFactory;
        private readonly IMessageSerializer _messageSerializer;

        public EventPublisher(ITopicClientFactory topicClientFactory, IMessageSerializer messageSerializer)
        {
            _topicClientFactory = topicClientFactory;
            _messageSerializer = messageSerializer;
        }

        public Task PublishAsync(object @event)
        {
            var topicName = TopicNameProvider.GetTopicName(@event.GetType());
            var message = _messageSerializer.Serialize(@event);
            var topicClient = _topicClientFactory.Create(topicName);

            return topicClient.SendAsync(message);
        }
    }
}
