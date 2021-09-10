using System;

using Messaging.DependencyInjection;
using Messaging.Factories;
using Messaging.RabbitMq.Configuration;
using Messaging.RabbitMq.Factories;
using Messaging.Serialization;

using Microsoft.Extensions.DependencyInjection;

namespace Messaging.RabbitMq.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IMessageHandlingBuilder AddRabbitMqMessaging(this IServiceCollection services, Action<RabbitMqConfiguration> configure)
        {
            services.AddCoreMessaging();

            services.AddSingleton<IQueueClientFactory, RabbitMqQueueClientFactory>();
            services.AddSingleton<ITopicClientFactory, RabbitMqTopicClientFactory>();

            services.AddTransient(typeof(IClientFactory<>), typeof(RabbitMqClientFactory<>));

            services.AddSingleton<IRabbitMqConfiguration, RabbitMqConfiguration>(_ =>
            {
                var configuration = new RabbitMqConfiguration();
                configure.Invoke(configuration);
                return configuration;
            });

            return new MessageHandlingBuilder(services);
        }
    }
}