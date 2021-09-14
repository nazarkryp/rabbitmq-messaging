using System.Threading.Tasks;

using Messaging.Demo.Messaging.Configurations;
using Messaging.Demo.Messaging.Handlers;
using Messaging.RabbitMq.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Messaging.Demo
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();

            await Task.WhenAll(
                host.RunAsync(),
                host.Services.GetRequiredService<DemoService>().DemoAsync()
            );
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    services
                        .AddTransient<DemoService>();

                    services
                        .AddRabbitMqMessaging(rabbitMqConfiguration =>
                        {
                            var rabbitMqConfigurationSection = configuration.GetSection("RabbitMqConfiguration");
                            rabbitMqConfiguration.AmqpUrl = rabbitMqConfigurationSection.GetValue<string>("AmqpUrl");
                            rabbitMqConfiguration.Password = rabbitMqConfigurationSection.GetValue<string>("Password");
                        })
                        .AddQueue<DemoQueueConfiguration>(queueConfiguration =>
                        {
                            queueConfiguration.QueueName = configuration.GetSection("DemoQueue").GetValue<string>("QueueName");
                        })
                        .AddTopic<DemoTopicConfiguration>(topicConfiguration =>
                        {
                            topicConfiguration.TopicName = configuration.GetSection("DemoTopic").GetValue<string>("TopicName");
                        })
                        .AddHandler<DemoCommandHandler>()
                        .AddHandler<DemoEventHandler>();
                });
        }
    }
}