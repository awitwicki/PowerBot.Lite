using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Handlers;
using Example.Services;
using PowerBot.Lite.Utils;

namespace Example
{
    public class BotHandler : BaseHandler
    {
        private readonly IRandomService _randomService;

        public BotHandler(IRandomService randomService)
        {
            _randomService = randomService;
        }

        [MessageReaction(ChatAction.Typing)]
        [MessageHandler("/start")]
        public Task Start()
        {
            var randomValue = _randomService.Random(0, 100);
            var userMention = User.GetUserMention();

            var messageText = $"Hi {userMention}! Random integer is: {randomValue}";

            return BotClient.SendTextMessageAsync(ChatId, messageText);
        }

        [MessageTypeFilter(MessageType.Text)]
        public Task TextMethod()
        {
            return BotClient.SendTextMessageAsync(ChatId, "Any text message trigger");
        }
    }
}
