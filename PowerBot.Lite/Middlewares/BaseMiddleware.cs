using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PowerBot.Lite.Middlewares
{
    public interface IBaseMiddleware
    {
        void PushNextMiddleware(IBaseMiddleware nextMiddleware);
        abstract Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func);
    }

    public abstract class BaseMiddleware : IBaseMiddleware
    {
        public IBaseMiddleware NextMiddleware { get; set; }

        public abstract Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func);

        public void PushNextMiddleware(IBaseMiddleware nextMiddleware)
        {
            if (NextMiddleware == null)
            {
                NextMiddleware = nextMiddleware;
            }
            else
            {
                NextMiddleware.PushNextMiddleware(nextMiddleware);
            }
        }
    }
}
