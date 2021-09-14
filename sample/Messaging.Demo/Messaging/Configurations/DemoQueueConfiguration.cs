using Messaging.Configuration;

namespace Messaging.Demo.Messaging.Configurations
{
    public class DemoQueueConfiguration : IQueueConfiguration
    {
        public string QueueName { get; set; }
    }
}
