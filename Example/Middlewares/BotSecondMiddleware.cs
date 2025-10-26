using Example.Services;
using PowerBot.Lite.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Example.Middlewares;

public class BotSecondMiddleware : BaseMiddleware
{
    private readonly IScopeTestService _scopeTestService;
    public BotSecondMiddleware(IScopeTestService scopeTestService)
    {
        _scopeTestService = scopeTestService;
    }
    
    public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
    {
        Console.WriteLine("SecondMiddleware before _nextMiddleware log");

        var scopeTestServiceId = _scopeTestService.GetId();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"SecondMiddleware, scopeTestServiceId is {scopeTestServiceId}");
        Console.ForegroundColor = ConsoleColor.White;

        await NextMiddleware.Invoke(bot, update, func);

        Console.WriteLine("SecondMiddleware after _nextMiddleware log");
    }
}
