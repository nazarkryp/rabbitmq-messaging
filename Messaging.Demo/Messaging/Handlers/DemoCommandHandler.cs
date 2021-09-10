using System;
using System.Threading.Tasks;

using Messaging.Clients;
using Messaging.Handlers;

namespace Messaging.Demo.Messaging.Handlers
{
    public class DemoCommandHandler : IMessageHandler<DemoCommand>
    {
        public Task HandleAsync(DemoCommand message)
        {
            Console.WriteLine("Handling DemoCommand 1. I ROCK!");

            Console.WriteLine(message);

            return Task.CompletedTask;
        }

        //public Task HandleAsync(object message)
        //{
        //    Console.WriteLine("Handling DemoCommand");

        //    Console.WriteLine(message);

        //    return Task.CompletedTask;
        //}
    }
}