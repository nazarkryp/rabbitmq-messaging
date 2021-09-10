using System;
using System.Threading;
using System.Threading.Tasks;

using Messaging.Services;

using Microsoft.Extensions.Hosting;

namespace Messaging.Hosting
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
            return _listenerService.StartListenersAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("BackgroundWorkerService .. stop");
            
            return Task.CompletedTask;
        }
    }
}