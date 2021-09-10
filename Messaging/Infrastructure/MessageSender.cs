using System.Threading.Tasks;

using Messaging.Clients;
using Messaging.Factories;
using Messaging.Serialization;

namespace Messaging.Infrastructure
{
    public interface IMessageSender<TConfiguration>
    {
        Task SendAsync(object @object);
    }

    public class MessageSender<TConfiguration> : IMessageSender<TConfiguration>
    {
        private readonly IClient _client;
        private readonly IMessageSerializer _messageSerializer;

        public MessageSender(IClientFactory<TConfiguration> clientFactory, IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer;
            _client = clientFactory.Create();
        }

        public Task SendAsync(object @object)
        {
            var message = _messageSerializer.Serialize(@object);

            return _client.SendAsync(message);
        }
    }
}