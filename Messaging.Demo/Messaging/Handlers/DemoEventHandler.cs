using System;
using System.Threading.Tasks;

using Messaging.Handlers;

namespace Messaging.Demo.Messaging.Handlers
{
    public class DemoEventHandler : IMessageHandler<DemoEvent>
    {
        public Task HandleAsync(DemoEvent message)
        {
            Console.WriteLine($"DemoEvent received: {message.Id}");
            return Task.CompletedTask;
        }
    }
}
