using PowerBot.Lite.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Example.Middlewares;

public class BotSecondMiddleware : BaseMiddleware
{
    public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
    {
        Console.WriteLine("SecondMiddleware before _nextMiddleware log");

        await NextMiddleware.Invoke(bot, update, func);

        Console.WriteLine("SecondMiddleware after _nextMiddleware log");
    }
}
