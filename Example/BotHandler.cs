using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Example.Services;

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

            var messageText = $"Hi! Random integer is: {randomValue}";

            return BotClient.SendTextMessageAsync(ChatId, messageText);
        }

        [MessageTypeFilter(MessageType.Text)]
        public Task TextMethod()
        {
            return BotClient.SendTextMessageAsync(ChatId, "Any text message trigger");
        }
    }
}
