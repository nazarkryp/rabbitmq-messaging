using System;
using System.Text.Json;

using NKryp.Messaging.Infrastructure.Attributes;

namespace NKryp.Messaging.Demo.Messaging
{
    [MessageKey("demo-event"), Topic("demo-topic")]
    public class DemoEvent
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
