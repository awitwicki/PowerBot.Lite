# PowerBot.Lite 

[Telegram bot](https://github.com/TelegramBots/Telegram.Bot) wrapper.

[![Tests](https://img.shields.io/nuget/v/PowerBot.Lite)](https://www.nuget.org/packages/PowerBot.Lite/)

[![Build and test](https://github.com/awitwicki/PowerBot.Lite/actions/workflows/dotnet.yml/badge.svg)](https://github.com/awitwicki/PowerBot.Lite/actions/workflows/dotnet.yml)
![Tests](https://img.shields.io/github/issues/awitwicki/PowerBot.Lite)
![Tests](https://img.shields.io/github/issues-pr/awitwicki/PowerBot.Lite)
![Tests](https://img.shields.io/github/last-commit/awitwicki/PowerBot.Lite)

![Tests](https://img.shields.io/github/languages/top/awitwicki/PowerBot.Lite)
![Tests](https://img.shields.io/badge/dotnet-8.0-blue)
![Tests](https://img.shields.io/github/stars/awitwicki/PowerBot.Lite)
![License](https://img.shields.io/badge/License-MIT-blue)

Unfortunately, I don't have much time to actively maintain the project, but I make occasional changes.
Fortunately, this library works well in my telegram bots and hasn't encountered major issues.
If you're interested or would like to contribute, please check out the code and feel free to join!
Welcome to the community and the development of this project.

## Main info
* Library is based on Telegram.Bot 19.0.0 and Telegram.Bot.Extensions.Polling with Version 1.0.2 which is referred to official Telegram Bot API version 7.0
* There is simple [Example](Example) project.
* Telegram [channel](https://t.me/powerbot_wrapper) with community.

## How to use

1. Add package to your project

2. Main code in Program.cs

```csharp
using PowerBot.Lite;

var botClient = new CoreBot("TOKEN")
    .RegisterHandler<SampleHandler>()
    .Build();

botClient.StartReveiving();

// Wait for eternity
await Task.Delay(Int32.MaxValue);
```

3. Create class that inherits `BaseHandler` class and define bot methods:

```csharp
class SampleHandler : BaseHandler
{
    [MessageReaction(ChatAction.Typing)]
    [MessageHandler("^/start$")]
    public async Task Start()
    {
        var messageText = $"Hi! your id is {User.TelegramId}, chatId is {ChatId}.";
        await BotClient.SendTextMessageAsync(ChatId, messageText);
    }

    [MessageReaction(ChatAction.Typing)]
    [MessageHandler("^/test$")]
    public async Task TestMethod()
    {
        var messageText = $"Test passed successfully!";
        await BotClient.SendTextMessageAsync(ChatId, messageText);
    }

    [MessageTypeFilter(MessageType.Voice)]
    public async Task VoiceMethod()
    {
        var messageText = $"Voice message!";
        await BotClient.SendTextMessageAsync(ChatId, messageText);
    }
}
```

`Powerbot.Lite` maps defined message handlers to incoming `telegram.Update` and run them when attribute filters are match.
`PowerBot.Lite` will find and run only the first one matched method from each defined class that inherits `BaseHandler` type.

## Methods attributes

For method matching `PowerBot.Lite` uses next attributes:

*Filter attributes ordered by priority

### `[MessageReaction(ChatAction.Typing)]`
Makes bot, send defined `ChatAction` before runs matched method.

### Message attribute filters:

### `[MessageHandler(string pattern)]`
Filter for text messages, `string pattern` - is regex matching pattern.

### `[CallbackQueryHandler(string dataPattern)]`
Filter for CallbackQuery events, `string dataPattern` - is regex matching pattern for `CallbackQuery.Data`. By default its equals `null` and matches any CallbackQuery event.

### `[MessageTypeFilter(MessageType messageType)]`
Filter for any messages by defined type (Text, Photo, Audio, Video, Voice, ...)

### `[UpdateTypeFilter(UpdateType updateType)]`
Filter for any updates by defined type (Message, InlineQuery, ChosenInlineResult, CallbackQuery, EditedMessage, ...)

## Custom Handler Filters

You can create custom filters to control whether a handler method should be executed. This is useful for implementing authorization checks, rate limiting, feature flags, or any other custom logic.

### 1. Create a filter class

Extend the `BaseHandlerFilter` class. It provides the same convenience properties as `BaseHandler` (`User`, `ChatId`, `Message`, `Chat`, etc.):

```csharp
public class AdminOnlyFilter : BaseHandlerFilter
{
    private static readonly HashSet<long> AdminIds = new() { 123456789 };

    protected override Task<bool> FilterAsync()
    {
        return Task.FromResult(AdminIds.Contains(User.Id));
    }
}
```

The `FilterAsync` method returns `true` to allow the handler to execute, or `false` to skip it.

Available properties inside the filter: `Update`, `User`, `Message`, `CallbackQuery`, `MessageId`, `Chat`, `ChatId`.

Alternatively, you can implement the `IHandlerFilter` interface directly if you only need the raw `Update`:

```csharp
public class RawFilter : IHandlerFilter
{
    public Task<bool> FilterAsync(Update update)
    {
        return Task.FromResult(update.Message?.Text != null);
    }
}
```

### 2. Apply the filter to a handler method

Use the `[HandlerFilter<T>]` attribute:

```csharp
[HandlerFilter<AdminOnlyFilter>]
[MessageHandler("/admin")]
public async Task AdminCommand()
{
    await BotClient.SendMessage(chatId: ChatId, text: "Admin panel");
}
```

### 3. Multiple filters

You can stack multiple filters on a single method. All filters must pass (AND logic) for the handler to execute:

```csharp
[HandlerFilter<AdminOnlyFilter>]
[HandlerFilter<RateLimitFilter>]
[MessageHandler("/secret")]
public async Task SecretCommand()
{
    await BotClient.SendMessage(chatId: ChatId, text: "Secret content");
}
```

Filters are **auto-registered** in the DI container — no manual registration needed. They are resolved per lifetime scope, so you can inject dependencies into your filter classes via constructor injection.

## Middleware

Also You can define your own middlewares by creating class that inherits `BaseMiddleware` and register it in CoreBot builder it by next template:

```csharp
var botClient = new CoreBot("TOKEN")
    .RegisterMiddleware<FirstMiddleware>()
    .RegisterMiddleware<SecondMiddleware>()
    ...
```

```csharp
public class FirstMiddleware : BaseMiddleware
{
    public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
    {
        Console.WriteLine("FirstMiddleware before _nextMiddleware log");

        // Do next middleware or process telegram.Update from defined handler if there no other defined middlewares.
        await _nextMiddleware.Invoke(bot, update, func);

        Console.WriteLine("FirstMiddleware after _nextMiddleware log");
    }
}
```

```csharp
public class SecondMiddleware : BaseMiddleware
{
    public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
    {
        Console.WriteLine("SecondMiddleware before _nextMiddleware log");

        // Do next middleware or process telegram.Update from defined handler if there no other defined middlewares.
        await _nextMiddleware.Invoke(bot, update, func);

        Console.WriteLine("SecondMiddleware after _nextMiddleware log");
    }
}
```

Console output:

```
FirstMiddleware before _nextMiddleware log
SecondMiddleware before _nextMiddleware log
SecondMiddleware after _nextMiddleware log
FirstMiddleware after _nextMiddleware log
```

## Dependency Injection

`PowerBot.Lite` supports dependency injection, based on Autofac container provider.

To use it, firstly you need to define your DI services as class with interface:

```csharp
public interface IRandomService
{
    public int Random(int min, int max);
}

public class RandomService : IRandomService
{
    private Random _random;

    public RandomService()
    {
        _random = new Random();
    }

    public int Random(int min, int max)
    {
        return _random.Next(min, max);
    }
}
```


And manually map them in `PowerBot.Lite` by using `RegisterContainers(...)` method:

```csharp
var botClient = new CoreBot(botToken)
    .RegisterMiddleware<BotMiddleware>()
    .RegisterHandler<BotHandler>()
    .RegisterContainers(x => {
        x.RegisterType<RandomService>()
            .As<IRandomService>()
            .SingleInstance();
    })
    .Build();
```

After that you can inject `IRandomService` to your handlers or middlewares:

```csharp
public class BotHandler : BaseHandler
{
    private readonly IRandomService _randomService;

    public BotHandler(IRandomService randomService)
    {
        _randomService = randomService;
    }

    [MessageReaction(ChatAction.Typing)]
    [MessageHandler("/start")]
    public Task Start()
    {
        var randomValue = _randomService.Random(0, 100);
        var messageText = $"Hi! Random integer is: {randomValue}";

        return BotClient.SendTextMessageAsync(ChatId, messageText);
    }
}

public class BotMiddleware : BaseMiddleware
{
    private readonly IRandomService _randomService;

    public BotMiddleware(IRandomService randomService)
    {
        _randomService = randomService;
    }

    public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
    {
        var randomValue = _randomService.Random(0, 100);
        Console.WriteLine($"Random number is {randomValue}");

        await _nextMiddleware.Invoke(bot, update, func);
    }
}
```
