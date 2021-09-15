using System;
using System.Threading.Tasks;

namespace NKryp.Messaging.Clients
{
    public interface IClient : IDisposable
    {
        Task SendAsync(IMessage command);

        void Subscribe(Func<IMessage, Task> asyncFunc);
    }
}