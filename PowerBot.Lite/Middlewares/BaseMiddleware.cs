using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public IBaseMiddleware _nextMiddleware { get; set; }

        public abstract Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func);

        public void PushNextMiddleware(IBaseMiddleware nextMiddleware)
        {
            if (_nextMiddleware == null)
            {
                _nextMiddleware = nextMiddleware;
            }
            else
            {
                _nextMiddleware.PushNextMiddleware(nextMiddleware);
            }
        }
    }
}
