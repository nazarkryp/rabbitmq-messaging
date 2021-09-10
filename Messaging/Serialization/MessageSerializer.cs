using System;
using System.Text;
using System.Text.Json;

using Messaging.Clients;

namespace Messaging.Serialization
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

            var message = new Message
            {
                Body = body,
                UserProperties = { ["Key"] = @object.GetType().AssemblyQualifiedName }
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
