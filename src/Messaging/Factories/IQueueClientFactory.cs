using Messaging.Clients;

namespace Messaging.Factories
{
    public interface IQueueClientFactory
    {
        IClient Create(string queueName);
    }
}
