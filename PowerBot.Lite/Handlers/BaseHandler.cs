using Telegram.Bot;
using Telegram.Bot.Types;

namespace PowerBot.Lite.Handlers
{
    public abstract class BaseHandler
    {
        public ITelegramBotClient BotClient { get; set; }
        public Update Update { get; set; }

        // User thad has sent message, or user that has clicked message
        public User User => CallbackQuery == null ? Message.From : CallbackQuery.From;
        public CallbackQuery CallbackQuery => Update.CallbackQuery;
        public Message Message => Update.Message ?? Update.CallbackQuery!.Message;
        public int MessageId => Message.MessageId;
        public Chat Chat => Message.Chat;
        public long ChatId => Message.Chat.Id;

        public void Init(ITelegramBotClient bot, Update update)
        {
            BotClient = bot;
            Update = update;
        }
    }
}
