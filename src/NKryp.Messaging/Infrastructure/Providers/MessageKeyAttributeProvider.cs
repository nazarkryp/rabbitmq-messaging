using System;
using System.Linq;

using NKryp.Messaging.Infrastructure.Attributes;

namespace NKryp.Messaging.Infrastructure.Providers
{
    public interface IMessageKeyProvider
    {
        string GetMessageKey(object message);
    }

    public sealed class MessageKeyAttributeProvider : IMessageKeyProvider
    {
        public string GetMessageKey(object message)
            => GetMessageKey(message.GetType());

        internal static string GetMessageKey(Type messageType)
            => messageType.GetCustomAttributes(typeof(MessageKeyAttribute), true).Cast<MessageKeyAttribute>().FirstOrDefault()?.MessageKey ?? messageType.Name;
    }
}
