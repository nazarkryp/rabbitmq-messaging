using NKryp.Messaging.Clients;

namespace NKryp.Messaging.Factories
{
    public interface ITopicClientFactory
    {
        IClient Create(string topicName, bool createNew = false);
    }
}
