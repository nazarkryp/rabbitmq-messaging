namespace NKryp.Messaging.RabbitMq.Configuration
{
    public interface IRabbitMqConfiguration
    {
        string Uri { get; }

        string Password { get; }
    }

    public class RabbitMqConfiguration : IRabbitMqConfiguration
    {
        public string Uri { get; set; }

        public string Password { get; set; }
    }
}