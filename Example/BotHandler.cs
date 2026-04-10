using Example.Filters;
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
        private readonly IScopeTestService _scopeTestService;

        public BotHandler(IRandomService randomService, IScopeTestService scopeTestService)
        {
            _randomService = randomService;
            _scopeTestService = scopeTestService;
        }

        [MessageReaction(ChatAction.Typing)]
        [MessageHandler("/start")]
        public async Task Start()
        {
            var scopeTestServiceId = _scopeTestService.GetId();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"/Start, scopeTestServiceId is {scopeTestServiceId}");
            Console.ForegroundColor = ConsoleColor.White;

            var randomValue = _randomService.Random(0, 100);
            var userMention = User.GetUserMention();

            var messageText = $"Hi {userMention}! Random integer is: {randomValue}";

            await BotClient.SendMessage(chatId: ChatId, text: messageText);

            await Task.Delay(5000);
            Console.WriteLine("End of /start method");
        }

        [HandlerFilter<OnlyPremiumUsersFilter>]
        [MessageReaction(ChatAction.Typing)]
        [MessageHandler("/premium")]
        public async Task FilteredMethod()
        {
            await Task.Delay(5000);
            await BotClient.SendMessage(chatId: ChatId, text: "Hello premium user!");
        }

        [MessageTypeFilter(MessageType.Text)]
        public Task TextMethod()
        {
            return BotClient.SendMessage(chatId: ChatId, text: "Any text message trigger");
        }
    }
}
