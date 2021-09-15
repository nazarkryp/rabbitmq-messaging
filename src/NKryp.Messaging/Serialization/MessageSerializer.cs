using System;
using System.Text;
using System.Text.Json;

using NKryp.Messaging.Clients;
using NKryp.Messaging.Infrastructure.Providers;

namespace NKryp.Messaging.Serialization
{
    public interface IMessageSerializer
    {
        IMessage Serialize(object @object);

        object Deserialize(IMessage message, Type type);
    }

    public class MessageSerializer : IMessageSerializer
    {
        public IMessage Serialize(object @object)
        {
            var json = JsonSerializer.Serialize(@object);
            var body = Encoding.UTF8.GetBytes(json);

            var key = MessageKeyAttributeProvider.GetMessageKey(@object.GetType());

            var message = new Message
            {
                Body = body,
                UserProperties = { ["Key"] = key }
            };

            return message;
        }

        public object Deserialize(IMessage message, Type type)
        {
            var msg = (Message)message;

            var json = Encoding.UTF8.GetString(msg.Body);

            var @object = JsonSerializer.Deserialize(json, type);

            return @object;
        }
    }
}
