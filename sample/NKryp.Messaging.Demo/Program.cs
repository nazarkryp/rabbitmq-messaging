using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NKryp.Messaging.Demo.Messaging.Handlers;
using NKryp.Messaging.RabbitMq.Infrastructure;

namespace NKryp.Messaging.Demo
{
    public static class Program
    {
        private static async Task Main()
        {
            var host = CreateHostBuilder()
                .Build();

            await Task.WhenAll(
                host.RunAsync()
                ,
                host.Services.GetRequiredService<DemoService>().DemoAsync()
            );
        }

        private static IHostBuilder CreateHostBuilder()
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
                            rabbitMqConfiguration.Uri = rabbitMqConfigurationSection.GetValue<string>("AmqpUrl");
                            rabbitMqConfiguration.Password = rabbitMqConfigurationSection.GetValue<string>("Password");
                        })
                        .AddHandler<DemoCommandHandler>()
                        .AddHandler<DemoEventHandler>()
                        .AddMessagingHostedService();
                });
        }
    }
}