using Messaging.Clients;

namespace Messaging.Factories
{
    public interface ITopicClientFactory
    {
        IClient Create(string topicName);
    }
}
