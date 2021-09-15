using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NKryp.Messaging.Services
{
    public class MessagingHostedService : IHostedService
    {
        private readonly IMessageListenerService _listenerService;
        private readonly ILogger<MessagingHostedService> _logger;

        public MessagingHostedService(IMessageListenerService listenerService, ILogger<MessagingHostedService> logger)
        {
            _listenerService = listenerService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting {hostedService}", nameof(MessagingHostedService));
            _listenerService.StartListeners();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping {hostedService}", nameof(MessageListenerService));
            _listenerService.StopListeners();

            return Task.CompletedTask;
        }
    }
}