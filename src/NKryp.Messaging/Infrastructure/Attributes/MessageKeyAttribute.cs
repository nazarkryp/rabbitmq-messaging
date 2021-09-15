using System;

namespace NKryp.Messaging.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MessageKeyAttribute : Attribute
    {
        public MessageKeyAttribute(string messageKey)
        {
            MessageKey = messageKey;
        }

        public string MessageKey { get; }
    }
}
