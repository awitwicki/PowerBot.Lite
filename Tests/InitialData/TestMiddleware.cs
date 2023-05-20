using System;
using System.Threading.Tasks;
using PowerBot.Lite.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Tests.InitialData;

public class TestMiddleware : BaseMiddleware
{
    public override Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
    {
        throw new NotImplementedException();
    }
}
