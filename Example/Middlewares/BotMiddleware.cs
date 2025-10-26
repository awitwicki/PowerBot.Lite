using Example.Services;
using PowerBot.Lite.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Example.Middlewares
{
    public class BotMiddleware : BaseMiddleware
    {
        private readonly IScopeTestService _scopeTestService;
        public BotMiddleware(IScopeTestService scopeTestService)
        {
            _scopeTestService = scopeTestService;
        }

        public override async Task Invoke(ITelegramBotClient bot, Update update, Func<Task> func)
        {
            Console.WriteLine("FirstMiddleware before _nextMiddleware log");

            var scopeTestServiceId = _scopeTestService.GetId();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FirstMiddleware, scopeTestServiceId is {scopeTestServiceId}");
            Console.ForegroundColor = ConsoleColor.White;
            
            await NextMiddleware.Invoke(bot, update, func);

            Console.WriteLine("FirstMiddleware after _nextMiddleware log");
        }
    }
}
