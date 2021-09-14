using System;

using Messaging.Handlers;

namespace Messaging.Factories
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler Create(Type messageType);
    }
}
