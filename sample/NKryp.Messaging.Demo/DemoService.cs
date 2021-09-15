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
            await SendCommand();

            //await Task.Delay(1000);
            //await SendCommand();
            //await Task.Delay(3000);
            //await SendCommand();
            // await SendEvent();
        }

        private async Task SendCommand()
        {
            var command = new DemoCommand
            {
                Id = Guid.NewGuid(),
                Content = "This is command message",
                Date = DateTime.Now
            };

            await _commandSender.SendAsync(command);
        }

        private async Task SendEvent()
        {
            var @event = new DemoEvent
            {
                Id = Guid.NewGuid(),
                Message = "This is some event message"
            };

            await _eventPublisher.PublishAsync(@event);
        }
    }
}