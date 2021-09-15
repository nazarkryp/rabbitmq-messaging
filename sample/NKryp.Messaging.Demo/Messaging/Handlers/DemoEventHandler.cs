using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Handlers;

namespace NKryp.Messaging.Demo.Messaging.Handlers
{
    public class DemoEventHandler : IMessageHandler<DemoEvent>
    {
        private readonly ILogger<DemoEventHandler> _logger;

        public DemoEventHandler(ILogger<DemoEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(DemoEvent @event)
        {
            _logger.LogInformation("\nEvent Received: {@event}\n", @event);

            return Task.CompletedTask;
        }
    }
}
