# NKryp.Messaging.RabbitMq

NKryp.Messaging.RabbitMq is a simple .NET Library that allows to easily implement messaging via RabbitMq queues and topic exchange.

## Installing via Nuget

```
Install-Package NKryp.Messaging.RabbitMq
```

## Usage

#### 1. Add Commands and Events

Add MessageKey *(optional)* - message identifier, queue - queue name, topic - topic name, attributes to your commands and messages.

```csharp
[MessageKey("demo-command"), Queue("demo-queue")]
public class DemoCommand
{
    public string Message { get; set; }
}

[MessageKey("demo-event"), Topic("demo-topic")]
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
     .AddHandler<DemoCommandHandler>()
     .AddHandler<DemoEventHandler>();
```

#### 4. Inject IMessageSender\<Configuration> to your service

```csharp
public class DemoService 
{
    private readonly ICommandSender _commandSender;
    private readonly IEventPublisher _eventPublisher;

    public DemoService(
        ICommandSender commandSender,
        IEventPublisher eventPublisher) 
    {
        _commandSender = commandSender;
        _eventPublisher = eventPublisher;
    }

    public async Task DoSomethingAsync()
    {
        await _commandSender.SendAsync(new DemoCommand {
            Message = "This is some test message"
        });

        await _eventPublisher.SendAsync(new DemoEvent {
            Message = "This is some event message"
        });
    }
}
```
