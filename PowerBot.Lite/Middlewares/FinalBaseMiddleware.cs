using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PowerBot.Lite.Middlewares
{
    internal class FinalBaseMiddleware : BaseMiddleware
    {
        public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> updateDelegate)
        {
            await updateDelegate();
        }
    }
}
