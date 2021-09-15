using System;

using Microsoft.Extensions.Logging;

using NKryp.Messaging.Infrastructure.Exceptions;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace NKryp.Messaging.RabbitMq.Clients
{
    public abstract class RabbitMqClientBase : IDisposable
    {
        private readonly Lazy<IConnection> _lazyConnection;
        private readonly Lazy<IModel> _lazyChannel;
        private readonly ILogger<RabbitMqClientBase> _logger;

        protected RabbitMqClientBase(string uri, string password, ILoggerFactory loggerFactory)
        {
            _lazyConnection = new Lazy<IConnection>(() => CreateConnection(uri, password));
            _lazyChannel = new Lazy<IModel>(() => _lazyConnection.Value.CreateModel());

            _logger = loggerFactory.CreateLogger<RabbitMqClientBase>();
        }

        protected IModel Channel => _lazyChannel.Value;

        private IConnection CreateConnection(string uri, string password)
        {
            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    Uri = new Uri(uri),
                    Password = password
                };

                return connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException exception)
            {
                _logger.LogError("Error establishing connection to {uri}", uri);

                throw new ConnectionException($"Error establishing connection to {uri}", exception);
            }
        }

        public void Dispose()
        {
            if (_lazyConnection.IsValueCreated)
            {
                Channel.Dispose();
                _lazyConnection.Value.Dispose();
            }
        }
    }
}
