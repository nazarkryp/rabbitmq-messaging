# NKryp.Messaging.RabbitMq

NKryp.Messaging.RabbitMq is a simple .NET Library that allows to easily implement messaging via RabbitMq queues and topic exchange.



## Installing via Nuget

```
Install-Package NKryp.Messaging.RabbitMq
```



## Usage

#### 1. Add Commands and Events

```csharp
public class DemoCommand
{
    public string Message { get; set; }
}

public class DemoEvent
{
    public string Message { get; set; }
}
```

#### 2. Add Handlers

```csharp
public class DemoCommandHandler : IMessageHandler<DemoCommand>
{
    public Task HandleAsync(DemoCommand command)
    {
        Console.WriteLine($"Demo command: {command.Message}");

        return Task.CompletedTask;
    }
}

public class DemoEventHandler : IMessageHandler<DemoEvent>
{
    public Task HandleAsync(DemoEvent demoEvent)
    {
        Console.WriteLine($"DemoEvent received: {demoEvent.Message}");

        return Task.CompletedTask;
    }
}
```

#### 3. Configure Services

```csharp
services
    .AddRabbitMqMessaging(rabbitMqConfiguration =>
    {
        rabbitMqConfiguration.AmqpUrl = "amqp://guest:guest@localhost:5672";
        rabbitMqConfiguration.Password = "guest";
     })
     .AddQueue<DemoQueueConfiguration>(queueConfiguration =>
     {
         queueConfiguration.QueueName = "demo-queue";
     })
     .AddTopic<DemoTopicConfiguration>(topicConfiguration =>
     {
         topicConfiguration.TopicName = "demo-topic";
     })
     .AddHandler<DemoCommandHandler>()
     .AddHandler<DemoEventHandler>();

```

#### 4. Inject IMessageSender\<Configuration> to your service

```csharp
public class DemoService 
{
    private readonly IMessageSender<DemoQueueConfiguration> _demoQueueSender;
    private readonly IMessageSender<DemoTopicConfiguration> _demoTopicSender;

    public DemoService(
        IMessageSender<DemoQueueConfiguration> demoQueueSender,
        IMessageSender<DemoTopicConfiguration> _demoTopicSender) 
    {
        _demoQueueSender = demoQueueSender;
        _demoTopicSender = demoTopicSender;
    }

    public async Task DoSomethingAsync()
    {
        await _demoQueueSender.SendAsync(new DemoCommand {
            Message = "This is some test message"
        });

        await _demoTopicSender.SendAsync(new DemoEvent {
            Message = "This is some event message"
        });
    }
}
```

