using NKryp.Messaging.Factories;
using NKryp.Messaging.Infrastructure;
using NKryp.Messaging.Infrastructure.Providers;
using NKryp.Messaging.Serialization;

using Microsoft.Extensions.DependencyInjection;

namespace NKryp.Messaging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IMessageHandlerFactory, MessageHandlerFactory>();
            services.AddTransient(typeof(IMessageSender<>), typeof(MessageSender<>));
            services.AddSingleton<IMessageSerializer, MessageSerializer>();

            services.AddSingleton<ICommandSender, CommandSender>();
            services.AddSingleton<IEventPublisher, EventPublisher>();

            services.AddSingleton<IMessageKeyProvider, MessageKeyAttributeProvider>();

            return services;
        }
    }
}
