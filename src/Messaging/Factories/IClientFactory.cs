using Messaging.Clients;

using Microsoft.Extensions.Options;

namespace Messaging.Factories
{
    public interface IClientFactory<TConfiguration>
    {
        IClient Create();
    }
}
