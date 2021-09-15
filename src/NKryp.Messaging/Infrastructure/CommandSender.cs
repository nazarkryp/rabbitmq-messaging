using System.Threading.Tasks;

using NKryp.Messaging.Factories;
using NKryp.Messaging.Infrastructure.Providers;
using NKryp.Messaging.Serialization;

namespace NKryp.Messaging.Infrastructure
{
    public interface ICommandSender
    {
        Task SendAsync(object command);
    }

    public class CommandSender : ICommandSender
    {
        private readonly IQueueClientFactory _queueClientFactory;
        private readonly IMessageSerializer _messageSerializer;

        public CommandSender(IQueueClientFactory queueClientFactory, IMessageSerializer messageSerializer)
        {
            _queueClientFactory = queueClientFactory;
            _messageSerializer = messageSerializer;
        }

        public Task SendAsync(object command)
        {
            var queueName = QueueNameProvider.GetQueueName(command.GetType());
            var message = _messageSerializer.Serialize(command);

            var queueClient = _queueClientFactory.Create(queueName);

            return queueClient.SendAsync(message);
        }
    }
}
