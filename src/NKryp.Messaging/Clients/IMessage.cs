using System.Collections.Generic;

namespace NKryp.Messaging.Clients
{
    public interface IMessage
    {
        public byte[] Body { get; set; }

        public Dictionary<string, string> UserProperties { get; set; }
    }

    public class Message : IMessage
    {
        public Message()
        {
            UserProperties = new Dictionary<string, string>();
        }

        public byte[] Body { get; set; }

        public Dictionary<string, string> UserProperties { get; set; }
    }
}