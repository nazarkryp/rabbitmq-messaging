using Microsoft.Extensions.DependencyInjection;

using NKryp.Messaging.Handlers;
using NKryp.Messaging.Services;

namespace NKryp.Messaging.DependencyInjection
{
    public interface IMessageHandlingBuilder
    {
        IMessageHandlingBuilder AddHandler<THandler>() where THandler : class, IMessageHandler;

        void AddMessagingHostedService();
    }

    public class MessageHandlingBuilder : IMessageHandlingBuilder
    {
        private readonly IServiceCollection _services;

        public MessageHandlingBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IMessageHandlingBuilder AddHandler<THandler>() where THandler : class, IMessageHandler
        {
            _services.AddTransient<THandler>();
            _services.AddTransient<IMessageHandler>(serviceProvider => serviceProvider.GetRequiredService<THandler>());

            return this;
        }

        public void AddMessagingHostedService()
        {
            _services
                .AddTransient<IMessageListenerService, MessageListenerService>()
                .AddHostedService<MessagingHostedService>();
        }
    }
}
