using System;

using NKryp.Messaging.DependencyInjection;
using NKryp.Messaging.Factories;
using NKryp.Messaging.RabbitMq.Configuration;
using NKryp.Messaging.RabbitMq.Factories;

using Microsoft.Extensions.DependencyInjection;

namespace NKryp.Messaging.RabbitMq.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IMessageHandlingBuilder AddRabbitMqMessaging(this IServiceCollection services, Action<RabbitMqConfiguration> configure)
        {
            services.AddCoreMessaging();

            services.AddScoped<IQueueClientFactory, RabbitMqQueueClientFactory>();
            services.AddSingleton<ITopicClientFactory, RabbitMqTopicClientFactory>();

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