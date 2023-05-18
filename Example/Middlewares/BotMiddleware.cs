using PowerBot.Lite.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Example.Middlewares
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
}
