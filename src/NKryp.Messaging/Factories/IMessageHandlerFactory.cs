using System;

using NKryp.Messaging.Handlers;

namespace NKryp.Messaging.Factories
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler Create(Type messageType);
    }
}
