using System;
using System.Threading.Tasks;

namespace Messaging.Clients
{
    public interface IClient : IDisposable
    {
        Task SendAsync(IMessage command);

        void Subscribe(Action<IMessage> action);
    }
}