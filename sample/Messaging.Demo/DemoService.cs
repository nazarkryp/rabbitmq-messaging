using System;
using System.Threading.Tasks;

using Messaging.Demo.Messaging;
using Messaging.Demo.Messaging.Configurations;
using Messaging.Infrastructure;

using Console = System.Console;

namespace Messaging.Demo
{
    public class DemoService
    {
        private readonly IMessageSender<DemoQueueConfiguration> _demoCommandSender;
        private readonly IMessageSender<DemoTopicConfiguration> _demoEventSender;

        public DemoService(
            IMessageSender<DemoQueueConfiguration> demoCommandSender, 
            IMessageSender<DemoTopicConfiguration> demoEventSender)
        {
            _demoCommandSender = demoCommandSender;
            _demoEventSender = demoEventSender;
        }

        public async Task DemoAsync()
        {
            //await SendCommand();
            await SendEvent();

            Console.WriteLine("Press Ctrl + C to stop program...");
            Console.ReadKey();
        }

        private async Task SendCommand()
        {
            string message;

            Console.WriteLine("Type something:");
            while (!string.IsNullOrEmpty(message = Console.ReadLine()))
            {
                var command = new DemoCommand
                {
                    Id = Guid.NewGuid(),
                    Content = message,
                    Date = DateTime.Now
                };

                await _demoCommandSender.SendAsync(command);

                Console.WriteLine("Messages Sent");
            }
        }

        private async Task SendEvent()
        {
            var @event = new DemoEvent
            {
                Id = Guid.NewGuid()
            };

            await _demoEventSender.SendAsync(@event);

            Console.WriteLine("EVENT SENT");
        }
    }
}