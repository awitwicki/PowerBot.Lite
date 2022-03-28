using PowerBot.Lite.Attributes;
using PowerBot.Lite.Attributes.AttributeValidators;
using PowerBot.Lite.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.HandlerInvokers
{
    public class MessageInvoker
    {
        public static List<MethodInfo> FilterHandlerMethods(IEnumerable<MethodInfo> methodsCollection, Update update)
        {
            List<MethodInfo> filteredMethods = new List<MethodInfo>();

            // Filter methods attributes
            foreach (var method in methodsCollection)
            {
                // TODO move this matching to other class

                // Update type have the most priority than the others
                if (method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(UpdateTypeFilter)))
                {
                    // Pattern matching for message text
                    if (!AttributeValidators.MatchUpdateType(method, update))
                    {
                        continue;
                    }
                    else
                    {
                        filteredMethods.Add(method);
                        continue;
                    }
                }

                // Message type have bigger priority
                // If method have universal update type attribute filter then check it
                if (update.Type == UpdateType.Message && method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(MessageTypeFilter)))
                {
                    // Pattern matching for message text
                    if (!AttributeValidators.MatchMessageType(method, update))
                    {
                        continue;
                    }
                    else
                    {
                        filteredMethods.Add(method);
                        continue;
                    }
                }

                // Matching for update type
                if (update.Type == UpdateType.Message && update.Message.Text != null && method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(MessageHandler)))
                {
                    // Pattern matching for message text
                    if (!AttributeValidators.MatchMessageHandlerMethod(method, update.Message.Text))
                    {
                        continue;
                    }

                }
                else if (update.Type == UpdateType.CallbackQuery && method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(CallbackQueryHandler)))
                {
                    // Pattern matching for message text
                    if (!AttributeValidators.MatchCallbackQueryHandlerMethod(method, update.CallbackQuery.Data))
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                filteredMethods.Add(method);
            }

            return filteredMethods;
        }
        public async static Task InvokeUpdate(ITelegramBotClient botClient, Update update)
        {
            // Get all handlers
            var handlers = ReflectiveEnumerator.GetEnumerableOfType<BaseHandler>();


            foreach (var handlerType in handlers)
            {
                // Find method in handler
                MethodInfo[] handlerMethods = handlerType.GetMethods();

                List<MethodInfo> filteredMethods = FilterHandlerMethods(handlerMethods, update);

                // Invoke methods
                foreach (var method in filteredMethods)
                {
                    try
                    {
                        // Get and send chatAction from attributes
                        var chatAction = AttributeValidators.GetChatActionAttributes(method);
                        if (chatAction.HasValue)
                        {
                            long chatId = update.Message?.Chat.Id! ?? update.CallbackQuery?.Message?.Chat.Id! ?? -1;
                            await botClient.SendChatActionAsync(chatId, chatAction.Value);
                        }

                        // Cast handler object
                        BaseHandler handler = (BaseHandler)Activator.CreateInstance(handlerType);

                        // Set params
                        handler.Init(botClient, update);

                        // Invoke method
                        await (Task)method.Invoke(handler, parameters: new object[] { });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invoker error: {ex}");
                    }
                }
            }
        }
    }
}
