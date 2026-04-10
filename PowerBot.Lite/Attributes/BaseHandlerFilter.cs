using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace PowerBot.Lite.Attributes
{
    public abstract class BaseHandlerFilter : IHandlerFilter
    {
        protected Update Update { get; private set; }

        // User that has sent message, or user that has clicked message
        protected User User => CallbackQuery == null ? Message.From : CallbackQuery.From;
        protected CallbackQuery CallbackQuery => Update.CallbackQuery;
        protected Message Message => Update.Message ?? Update.CallbackQuery!.Message;
        protected int MessageId => Message.MessageId;
        protected Chat Chat => Message.Chat;
        protected long ChatId => Message.Chat.Id;

        public Task<bool> FilterAsync(Update update)
        {
            Update = update;
            return FilterAsync();
        }

        protected abstract Task<bool> FilterAsync();
    }
}
