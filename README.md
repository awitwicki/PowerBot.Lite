# PowerBot.Lite

[Telegram bot](https://github.com/TelegramBots/Telegram.Bot) wrapper.

```diff
- This project is in early stage!
```

## How to use

1. Add package to your project

2. Create class that inherits `BaseHandler` class and define bot methods:

```csharp
class SampleHandler : BaseHandler
{
    [MessageReaction(ChatAction.Typing)]
    [MessagePattern("/start")]
    public async Task Start()
    {
        string messageText = $"Hi! your id is {User.TelegramId}, chatId is {ChatId}.";
        await Bot.SendTextMessageAsync(ChatId, messageText);
    }

    [MessageReaction(ChatAction.Typing)]
    [MessagePattern("/test")]
    public async Task TestMethod()
    {
        string messageText = $"Test passed successfully!";
        await Bot.SendTextMessageAsync(ChatId, messageText);
    }

    [MessageTypeFilter(MessageType.Voice)]
    public async Task VoiceMethod()
    {
        string messageText = $"Voice message!";
        await Bot.SendTextMessageAsync(ChatId, messageText);
    }
}
```

## Example usage

```csharp
static void Main(string[] args)
{
    Console.WriteLine("Starting PowerBot.Lite");

    string botToken = Environment.GetEnvironmentVariable("PowerBot.Lite_TELEGRAM_TOKEN");

    if (botToken == null)
    {
        throw new Exception("ENV PowerBot.Lite_TELEGRAM_TOKEN is not defined");
    }

    CoreBot botClient = new CoreBot(botToken);

    // Wait for eternity
    Task.Delay(Int32.MaxValue).Wait();
}
```