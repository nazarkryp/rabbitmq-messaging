using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Handlers;

namespace NKryp.Messaging.Demo.Messaging.Handlers
{
    public class DemoCommandHandler : IMessageHandler<DemoCommand>
    {
        private readonly ILogger<DemoCommandHandler> _logger;

        public DemoCommandHandler(ILogger<DemoCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(DemoCommand command)
        {
            _logger.LogInformation("\nCommand Received: {command}\n", command);

            return Task.CompletedTask;
        }
    }
}