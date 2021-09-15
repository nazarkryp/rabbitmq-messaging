using System;

namespace NKryp.Messaging.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TopicAttribute : Attribute
    {
        public TopicAttribute(string topic)
        {
            Topic = topic;
        }

        public string Topic { get; }
    }
}
