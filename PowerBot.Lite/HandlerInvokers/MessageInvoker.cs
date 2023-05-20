using PowerBot.Lite.Attributes.AttributeValidators;
using PowerBot.Lite.Handlers;
using PowerBot.Lite.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Autofac;

namespace PowerBot.Lite.HandlerInvokers
{
    internal static class MessageInvoker
    {
        public static async Task InvokeUpdate(
            ITelegramBotClient botClient,
            Update update,
            IEnumerable<FastMethodInfo> handlerMethods)
        {
            foreach (var fastMethodInfo in handlerMethods)
            {
                try
                {
                    // Get and send chatAction from attributes
                    var chatAction = AttributeValidators.GetChatActionAttributes(fastMethodInfo.GetMethodInfo());
                    if (chatAction.HasValue)
                    {
                        var chatId = update.Message?.Chat.Id! ?? update.CallbackQuery?.Message?.Chat.Id! ?? -1;
                        await botClient.SendChatActionAsync(chatId, chatAction.Value);
                    }

                    await using (var scope = DIContainerInstance.Container.BeginLifetimeScope())
                    {
                        // Cast handler object
                        var handler = scope.ResolveNamed(serviceName: fastMethodInfo.GetHandlerType().Name,
                            fastMethodInfo.GetHandlerType());

                        // Set params
                        ((BaseHandler)handler).Init(botClient, update);

                        // Invoke method
                        fastMethodInfo.Invoke(handler);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invoker error: {ex}");
                }
            }
        }
    }
}
