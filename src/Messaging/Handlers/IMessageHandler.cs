using System.Threading.Tasks;

namespace Messaging.Handlers
{
    public interface IMessageHandler
    {
        Task HandleAsync(object message);
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler
    {
        Task IMessageHandler.HandleAsync(object message)
        {
            return HandleAsync((TMessage) message);
        }

        Task HandleAsync(TMessage message);
    }
}