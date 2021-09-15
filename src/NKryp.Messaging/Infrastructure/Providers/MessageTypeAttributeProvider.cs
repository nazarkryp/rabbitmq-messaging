using System;
using System.Collections.Generic;
using System.Linq;

namespace NKryp.Messaging.Infrastructure.Providers
{
    public interface IMessageTypeProvider
    {
        Type GetMessageType(string messageKey);
    }

    internal sealed class MessageTypeAttributeProvider : IMessageTypeProvider
    {
        private readonly Lazy<IReadOnlyDictionary<string, Type>> _mappings;

        public MessageTypeAttributeProvider(IEnumerable<Type> types)
        {
            _mappings = new Lazy<IReadOnlyDictionary<string, Type>>(() => Load(types), true);
        }

        public Type GetMessageType(string messageKey)
            => _mappings.Value.TryGetValue(messageKey, out Type messageType) ? messageType : null;

        private static IReadOnlyDictionary<string, Type> Load(IEnumerable<Type> types)
            => types.Select(e => new { Type = e, Key = MessageKeyAttributeProvider.GetMessageKey(e) })
                .Where(e => e.Key != null).ToDictionary(e => e.Key, e => e.Type);
    }
}
