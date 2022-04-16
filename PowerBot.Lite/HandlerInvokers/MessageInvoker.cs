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
        private static MethodInfo _matchHandlerMethodWithCallbackQueries(MethodInfo method, Update update)
        {
            // TODO move this matching to other class
            // Matching for message text regex have the most priority than the others
            if (method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(CallbackQueryHandler)))
            {
                // Pattern matching for CallbackQuery data
                if (AttributeValidators.MatchCallbackQueryHandlerMethod(method, update.CallbackQuery.Data))
                {
                    return method;
                }
            }

            return null;
        }

        private static MethodInfo _matchHandlerMethodWithMessages(MethodInfo method, Update update)
        {
            // TODO move this matching to other class
            // Matching for message text regex have the most priority than the others
            if (update.Message.Text != null && method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(MessageHandler)))
            {
                // Pattern matching for message text
                if (AttributeValidators.MatchMessageHandlerMethod(method, update.Message.Text))
                {
                    return method;
                }
            }

            // Message type
            // If method have universal update type attribute filter then check it
            if (update.Type == UpdateType.Message && method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(MessageTypeFilter)))
            {
                // Pattern matching for message text
                if (AttributeValidators.MatchMessageType(method, update))
                {
                    return method;
                }
            }

            return null;
        }

        public static MethodInfo MatchHandlerMethod(IEnumerable<MethodInfo> methodsCollection, Update update)
        {
            MethodInfo matchedMethod = null;

            // Filter methods attributes
            foreach (var method in methodsCollection)
            {
                switch (update.Type)
                {
                    case UpdateType.CallbackQuery:
                        matchedMethod = _matchHandlerMethodWithCallbackQueries(method, update);
                        break;

                    case UpdateType.Message:
                        matchedMethod = _matchHandlerMethodWithMessages(method, update);
                        break;

                    // TODO: other event types
                    // ...
                }

                // Check for update type
                if (matchedMethod == null)
                {
                    // Update type 
                    if (method.GetCustomAttributes(true).Any(y => y.GetType() == typeof(UpdateTypeFilter)))
                    {
                        // Pattern matching for message text
                        if (!AttributeValidators.MatchUpdateType(method, update))
                        {
                            continue;
                        }
                        else
                        {
                            return method;
                        }
                    }
                }

                if (matchedMethod != null)
                {
                    return matchedMethod;
                }
            }

            return matchedMethod;
        }
        public async static Task InvokeUpdate(ITelegramBotClient botClient, Update update)
        {
            // Get all handlers
            var handlers = ReflectiveEnumerator.GetEnumerableOfType<BaseHandler>();

            foreach (var handlerType in handlers)
            {
                // Find method in handler
                MethodInfo[] handlerMethods = handlerType.GetMethods();

                MethodInfo method = MatchHandlerMethod(handlerMethods, update);

                // Invoke first matched method
                if (method != null)
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
