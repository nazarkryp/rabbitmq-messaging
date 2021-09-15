using System;
using System.Text.Json;

using NKryp.Messaging.Infrastructure.Attributes;

namespace NKryp.Messaging.Demo.Messaging
{
    [Queue("demo-queue")]
    public class DemoCommand
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