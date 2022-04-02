using PowerBot.Lite.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PowerBot.Lite.HandlerInvokers
{
    public class MiddlewareInvoker
    {
        public async static Task InvokeUpdate(ITelegramBotClient botClient, Update update)
        {
            // Get all handlers
            var handlers = ReflectiveEnumerator.GetEnumerableOfType<BaseMiddleware>();

            Type handlerType = handlers.FirstOrDefault();

            if (handlerType != null)
            {
                try
                {
                    // Cast handler object
                    BaseMiddleware handler = (BaseMiddleware)Activator.CreateInstance(handlerType);

                    // Set params
                    handler.Init(botClient, update);

                    // Invoke method
                    await handler.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Middleware invoker error: {ex}");
                }
            }
        }
    }
}
