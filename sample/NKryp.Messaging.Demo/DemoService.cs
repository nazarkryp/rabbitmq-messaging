using System;
using System.Threading.Tasks;

using NKryp.Messaging.Demo.Messaging;
using NKryp.Messaging.Infrastructure;

namespace NKryp.Messaging.Demo
{
    public class DemoService
    {
        private readonly ICommandSender _commandSender;
        private readonly IEventPublisher _eventPublisher;

        public DemoService(ICommandSender commandSender, IEventPublisher eventPublisher)
        {
            _commandSender = commandSender;
            _eventPublisher = eventPublisher;
        }

        public async Task DemoAsync()
        {
            await SendCommand();
            await SendEvent();
            await SendEvent();
        }

        private async Task SendCommand()
        {
            var command = new DemoCommand
            {
                Id = Guid.NewGuid(),
                Message = "This is command message",
                Date = DateTime.Now
            };

            try
            {
                await _commandSender.SendAsync(command);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private async Task SendEvent()
        {
            var @event = new DemoEvent
            {
                Id = Guid.NewGuid(),
                Message = "This is some event message",
                Date = DateTime.Now
            };

            await _eventPublisher.PublishAsync(@event);
        }
    }
}