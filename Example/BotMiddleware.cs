using PowerBot.Lite.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Example
{
    public class BotMiddleware : BaseMiddleware
    {
        public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
        {
            Console.WriteLine("FirstMiddleware before _nextMiddleware log");

            await NextMiddleware.Invoke(bot, update, func);

            Console.WriteLine("FirstMiddleware after _nextMiddleware log");
        }
    }

    public class BotSecondMiddleware : BaseMiddleware
    {
        public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
        {
            Console.WriteLine("SecondMiddleware before _nextMiddleware log");

            await NextMiddleware.Invoke(bot, update, func);

            Console.WriteLine("SecondMiddleware after _nextMiddleware log");
        }
    }
}
