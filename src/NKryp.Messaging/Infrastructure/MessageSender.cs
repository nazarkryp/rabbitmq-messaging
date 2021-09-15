using System.Threading.Tasks;

using NKryp.Messaging.Clients;
using NKryp.Messaging.Factories;
using NKryp.Messaging.Serialization;

namespace NKryp.Messaging.Infrastructure
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