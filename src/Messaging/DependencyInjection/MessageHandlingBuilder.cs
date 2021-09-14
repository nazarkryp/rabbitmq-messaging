using System;

using Messaging.Configuration;
using Messaging.Handlers;
using Messaging.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Messaging.DependencyInjection
{
    public interface IMessageHandlingBuilder
    {
        IMessageHandlingBuilder AddQueue<TConfig>(Action<TConfig> configure) where TConfig : class, IQueueConfiguration;

        IMessageHandlingBuilder AddTopic<TConfig>(Action<TConfig> configure) where TConfig : class, ITopicConfiguration;

        IMessageHandlingBuilder AddHandler<THandler>() where THandler : class, IMessageHandler;
    }

    public class MessageHandlingBuilder : IMessageHandlingBuilder
    {
        private readonly IServiceCollection _services;
        private bool _hasListener;

        public MessageHandlingBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IMessageHandlingBuilder AddQueue<TConfig>(Action<TConfig> configure) where TConfig : class, IQueueConfiguration
        {
            _services.AddSingleton(typeof(TConfig));
            _services.AddSingleton(typeof(IQueueConfiguration), _ =>
            {
                var tconfig = _.GetService<TConfig>();
                configure.Invoke(tconfig);
                return tconfig;
            });

            _services.Configure<TConfig>(configure.Invoke);

            return this;
        }

        public IMessageHandlingBuilder AddTopic<TConfig>(Action<TConfig> configure) where TConfig : class, ITopicConfiguration
        {
            _services.AddSingleton(typeof(TConfig));
            _services.AddSingleton(typeof(ITopicConfiguration), _ =>
            {
                var tconfig = _.GetService<TConfig>();
                configure.Invoke(tconfig);
                return tconfig;
            });

            _services.Configure<TConfig>(configure.Invoke);

            return this;
        }

        public IMessageHandlingBuilder AddHandler<THandler>() where THandler : class, IMessageHandler
        {
            _services.Add(new ServiceDescriptor(typeof(THandler), typeof(THandler), ServiceLifetime.Transient));

            if (!_hasListener)
            {
                AddListeners();
                _hasListener = true;
            }

            return this;
        }

        private void AddListeners()
        {
            _services
                .AddTransient<MessageListenerService>()
                .AddHostedService<BackgroundWorkerService>();
        }
    }
}
