using NKryp.Messaging.Clients;

namespace NKryp.Messaging.Factories
{
    public interface IQueueClientFactory
    {
        IClient Create(string queueName, bool createNew = false);
    }
}
