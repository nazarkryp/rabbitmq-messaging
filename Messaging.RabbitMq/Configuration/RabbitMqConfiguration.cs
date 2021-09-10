namespace Messaging.RabbitMq.Configuration
{
    public interface IRabbitMqConfiguration
    {
        string AmqpUrl { get; }

        string Password { get; }
    }

    public class RabbitMqConfiguration : IRabbitMqConfiguration
    {
        public string AmqpUrl { get; set; }

        public string Password { get; set; }
    }
}