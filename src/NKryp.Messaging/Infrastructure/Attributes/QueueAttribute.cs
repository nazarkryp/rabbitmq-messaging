using System;

namespace NKryp.Messaging.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QueueAttribute : Attribute
    {
        public QueueAttribute(string queue)
        {
            Queue = queue;
        }

        public string Queue { get; }
    }
}
