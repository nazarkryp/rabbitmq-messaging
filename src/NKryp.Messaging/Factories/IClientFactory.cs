using NKryp.Messaging.Clients;

using Microsoft.Extensions.Options;

namespace NKryp.Messaging.Factories
{
    public interface IClientFactory<TConfiguration>
    {
        IClient Create();
    }
}
