using System;
using System.Linq;

using NKryp.Messaging.Infrastructure.Attributes;

namespace NKryp.Messaging.Infrastructure.Providers
{
    public class TopicNameProvider
    {
        public static string GetTopicName(Type commandType) => commandType.GetCustomAttributes(typeof(TopicAttribute), true).Cast<TopicAttribute>().FirstOrDefault()?.Topic;
    }
}
