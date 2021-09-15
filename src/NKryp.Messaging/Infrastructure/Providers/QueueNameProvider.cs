using System;
using System.Linq;

using NKryp.Messaging.Infrastructure.Attributes;

namespace NKryp.Messaging.Infrastructure.Providers
{
    internal static class QueueNameProvider
    {
        public static string GetQueueName(Type commandType) => commandType.GetCustomAttributes(typeof(QueueAttribute), true).Cast<QueueAttribute>().FirstOrDefault()?.Queue;
    }
}
