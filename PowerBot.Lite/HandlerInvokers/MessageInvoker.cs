using PowerBot.Lite.Attributes;
using PowerBot.Lite.Attributes.AttributeValidators;
using PowerBot.Lite.Handlers;
using PowerBot.Lite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Autofac;

namespace PowerBot.Lite.HandlerInvokers
{
    public static class MessageInvoker
    {
        public static List<HandlerDescriptor> CollectHandlers()
        {
            List<HandlerDescriptor> handlerDescriptors = new();

            // Get all handlers
            IEnumerable<Type> handlers = ReflectiveEnumerator.GetEnumerableOfType<BaseHandler>();

            foreach (var handlerType in handlers)
            {
                // Find method in handler
                MethodInfo[] handlerMethods = handlerType
                    .GetMethods()
                    .Where(x => x.DeclaringType != typeof(BaseHandler))
                    .Where(x => x.DeclaringType != typeof(Object))
                    .ToArray();

                IEnumerable<FastMethodInfo> fastMethodInfos = handlerMethods.Select(x => new FastMethodInfo(x, handlerType));

                handlerDescriptors.Add(new HandlerDescriptor(handlerType, fastMethodInfos));
            }

            return handlerDescriptors;
        }

        private static FastMethodInfo _matchHandlerMethodWithCallbackQueries(FastMethodInfo fastMethodInfo, Update update)
        {
            // TODO move this matching to other class
            // Matching for message text regex have the most priority than the others
            if (fastMethodInfo.GetMethodInfo().GetCustomAttributes(true).Any(y => y.GetType() == typeof(CallbackQueryHandlerAttribute)))
            {
                // Pattern matching for CallbackQuery data
                if (AttributeValidators.MatchCallbackQueryHandlerMethod(fastMethodInfo.GetMethodInfo(), update.CallbackQuery.Data))
                {
                    return fastMethodInfo;
                }
            }

            return null;
        }

        private static FastMethodInfo _matchHandlerMethodWithMessages(FastMethodInfo fastMethodInfo, Update update)
        {
            // TODO move this matching to other class
            // Matching for message text regex have the most priority than the others
            if (update.Message.Text != null && fastMethodInfo.GetMethodInfo().GetCustomAttributes(true).Any(y => y.GetType() == typeof(MessageHandler)))
            {
                // Pattern matching for message text
                if (AttributeValidators.MatchMessageHandlerMethod(fastMethodInfo.GetMethodInfo(), update.Message.Text))
                {
                    return fastMethodInfo;
                }
            }

            // Message type
            // If method have universal update type attribute filter then check it
            if (update.Type == UpdateType.Message && fastMethodInfo.GetMethodInfo().GetCustomAttributes(true).Any(y => y.GetType() == typeof(MessageTypeFilter)))
            {
                // Pattern matching for message text
                if (AttributeValidators.MatchMessageType(fastMethodInfo.GetMethodInfo(), update))
                {
                    return fastMethodInfo;
                }
            }

            return null;
        }

        public static FastMethodInfo MatchHandlerMethod(IEnumerable<FastMethodInfo> fastMethodInfosCollection, Update update)
        {
            FastMethodInfo matchedMethod = null;

            // Filter methods attributes
            foreach (var fastMethodInfo in fastMethodInfosCollection)
            {
                switch (update.Type)
                {
                    case UpdateType.CallbackQuery:
                        matchedMethod = _matchHandlerMethodWithCallbackQueries(fastMethodInfo, update);
                        break;

                    case UpdateType.Message:
                        matchedMethod = _matchHandlerMethodWithMessages(fastMethodInfo, update);
                        break;

                    // TODO: other event types
                    // ...
                }

                // Check for update type
                if (matchedMethod == null)
                {
                    // Update type 
                    if (fastMethodInfo.GetMethodInfo().GetCustomAttributes(true).Any(y => y.GetType() == typeof(UpdateTypeFilterAttribute)))
                    {
                        // Pattern matching for message text
                        if (!AttributeValidators.MatchUpdateType(fastMethodInfo.GetMethodInfo(), update))
                        {
                            continue;
                        }
                        else
                        {
                            return fastMethodInfo;
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

        public static IEnumerable<FastMethodInfo> FilterFastMethods(Update update, IEnumerable<HandlerDescriptor> handlerDescriptors)
        {
            return handlerDescriptors
                .Select(x => MatchHandlerMethod(x.GetMethodInfos(), update))
                .Where(x => x is not null)
                .ToList();
        }

        public async static Task InvokeUpdate(ITelegramBotClient botClient, Update update, IEnumerable<FastMethodInfo> handlerMethods)
        {
            foreach (FastMethodInfo fastMethodInfo in handlerMethods)
            {
                try
                {
                    // Get and send chatAction from attributes
                    var chatAction = AttributeValidators.GetChatActionAttributes(fastMethodInfo.GetMethodInfo());
                    if (chatAction.HasValue)
                    {
                        long chatId = update.Message?.Chat.Id! ?? update.CallbackQuery?.Message?.Chat.Id! ?? -1;
                        await botClient.SendChatActionAsync(chatId, chatAction.Value);
                    }

                    using (var scope = DIContainerInstance.Container.BeginLifetimeScope())
                    {
                        // Cast handler object
                        var handler = scope.ResolveNamed(serviceName: fastMethodInfo.GetHandlerType().Name, fastMethodInfo.GetHandlerType());

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
