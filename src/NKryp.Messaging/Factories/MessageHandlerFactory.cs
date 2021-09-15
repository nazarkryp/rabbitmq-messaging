using System;
using System.Linq;
using System.Reflection;

using NKryp.Messaging.Handlers;

namespace NKryp.Messaging.Factories
{
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessageHandler Create(Type type)
        {
            var implementationType = Assembly.GetAssembly(type).GetTypes()
                .FirstOrDefault(t => t.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GenericTypeArguments.First() == type));

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType), "Cannot create IMessageHandler. Implementation not found.");
            }

            var handler = _serviceProvider.GetService(implementationType);

            return (IMessageHandler)handler;
        }
    }
}
