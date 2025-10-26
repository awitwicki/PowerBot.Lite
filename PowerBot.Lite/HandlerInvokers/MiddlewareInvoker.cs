using Autofac;
using PowerBot.Lite.Middlewares;
using PowerBot.Lite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PowerBot.Lite.HandlerInvokers
{
    public static class MiddlewareInvoker
    {
        public static async Task InvokeUpdate(ITelegramBotClient botClient, Update update, Func<Task> processMethods, ILifetimeScope scope)
        {
            try
            {
                var middlewares = scope.Resolve<IEnumerable<IBaseMiddleware>>()
                    .ToArray();

                // Without middlewares
                if (!middlewares.Any())
                {
                    await processMethods();
                    return;
                }

                // Recursively create middlewares action execute tree
                var firstMiddleware = middlewares.First();

                // Push middlewares to tree
                foreach (var middleware in middlewares.Skip(1))
                {
                    firstMiddleware.PushNextMiddleware(middleware);
                }

                // Push final middleware to properly process processMethods delegate
                firstMiddleware.PushNextMiddleware(new FinalBaseMiddleware());

                // Run first middleware
                await firstMiddleware.Invoke(botClient, update, processMethods);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Middleware invoker error: {ex}");
            }
        }
    }
}
