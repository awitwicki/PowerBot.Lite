using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PowerBot.Lite.Middlewares
{
    // TODO: This is a PREMiddleware, need to rework to normal middleware conception
    public interface IBaseMiddleware
    {
        void Init(ITelegramBotClient bot, Update update);
        abstract Task Invoke();
    }

    public abstract class BaseMiddleware : IBaseMiddleware
    {
        public ITelegramBotClient BotClient { get; set; }
        public Update Update { get; set; }

        // User thad has sended message, or user that has clicked message
        public User User => CallbackQuery == null ? Message.From : CallbackQuery.From;
        public CallbackQuery CallbackQuery => Update.CallbackQuery;
        public Message Message => Update.Message ?? Update.CallbackQuery.Message;
        public int MessageId => Message.MessageId;
        public Chat Chat => Message.Chat;
        public long ChatId => Message.Chat.Id;

        public void Init(ITelegramBotClient bot, Update update)
        {
            BotClient = bot;
            Update = update;
        }

        public abstract Task Invoke();
    }
}
