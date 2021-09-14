using Messaging.Factories;
using Messaging.Infrastructure;
using Messaging.Serialization;

using Microsoft.Extensions.DependencyInjection;

namespace Messaging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IMessageHandlerFactory, MessageHandlerFactory>();
            services.AddTransient(typeof(IMessageSender<>), typeof(MessageSender<>));
            services.AddSingleton<IMessageSerializer, MessageSerializer>();

            return services;
        }
    }
}
