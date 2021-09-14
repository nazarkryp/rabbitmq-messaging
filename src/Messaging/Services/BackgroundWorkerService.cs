using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Messaging.Services
{
    public class BackgroundWorkerService : IHostedService
    {
        private readonly MessageListenerService _listenerService;

        public BackgroundWorkerService(MessageListenerService listenerService)
        {
            _listenerService = listenerService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _listenerService.StartListeners();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("BackgroundWorkerService .. stop");

            _listenerService.StopListeners();

            return Task.CompletedTask;
        }
    }
}