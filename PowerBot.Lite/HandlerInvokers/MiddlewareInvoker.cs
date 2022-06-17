using Autofac;
using PowerBot.Lite.Middlewares;
using PowerBot.Lite.Services;
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
    public static class MiddlewareInvoker
    {
        public async static Task InvokeUpdate(ITelegramBotClient botClient, Update update)
        {
            using (var scope = DIContainerInstance.Container.BeginLifetimeScope())
            {
                var middlewares = scope.Resolve<IEnumerable<IBaseMiddleware>>();

                foreach (IBaseMiddleware middleware in middlewares)
                {
                    if (middleware != null)
                    {
                        try
                        {
                            // Set params
                            middleware.Init(botClient, update);

                            // Invoke method
                            await middleware.Invoke();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Middleware invoker error: {ex}");
                        }
                    }
                }
            }
        }
    }
}
