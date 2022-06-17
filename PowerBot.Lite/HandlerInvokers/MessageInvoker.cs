using PowerBot.Lite.Attributes;
using PowerBot.Lite.Attributes.AttributeValidators;
using PowerBot.Lite.Handlers;
using PowerBot.Lite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using System.Linq.Expressions;
using Autofac;

namespace PowerBot.Lite.HandlerInvokers
{

    public class FastMethodInfot
    {
        private delegate object ReturnValueDelegate(object instance, object[] arguments);
        //private delegate object Delegate(object instance, object[] arguments);
        private delegate void VoidDelegate(object instance, object[] arguments);

        public FastMethodInfot(MethodInfo methodInfo)
        {
            var instanceExpression = Expression.Parameter(typeof(object), "instance");
            var argumentsExpression = Expression.Parameter(typeof(object[]), "arguments");
            var argumentExpressions = new List<Expression>();
            var parameterInfos = methodInfo.GetParameters();
            for (var i = 0; i < parameterInfos.Length; ++i)
            {
                var parameterInfo = parameterInfos[i];
                argumentExpressions.Add(Expression.Convert(Expression.ArrayIndex(argumentsExpression, Expression.Constant(i)), parameterInfo.ParameterType));
            }
            var callExpression = Expression.Call(!methodInfo.IsStatic ? Expression.Convert(instanceExpression, methodInfo.ReflectedType) : null, methodInfo, argumentExpressions);
            if (callExpression.Type == typeof(void))
            {
                var voidDelegate = Expression.Lambda<VoidDelegate>(callExpression, instanceExpression, argumentsExpression).Compile();
                Delegate = (instance, arguments) => { voidDelegate(instance, arguments); return null; };
            }
            else
                Delegate = Expression.Lambda<ReturnValueDelegate>(Expression.Convert(callExpression, typeof(object)), instanceExpression, argumentsExpression).Compile();
        }

        private ReturnValueDelegate Delegate { get; }

        public object Invoke(object instance, params object[] arguments)
        {
            return Delegate(instance, arguments);
        }
    }

    public static class MessageInvoker
    {
        public static List<HandlerDescriptor> CollectHandlers()
        {
            List<HandlerDescriptor> handlerDescriptors = new List<HandlerDescriptor>();

            // Get all handlers
            IEnumerable<Type> handlers = ReflectiveEnumerator.GetEnumerableOfType<BaseHandler>();

            foreach (var handlerType in handlers)
            {
                // Find method in handler
                MethodInfo[] handlerMethods = handlerType
                    .GetMethods()
                    .Where(m => !m.IsSpecialName)
                    .ToArray();

                IEnumerable<FastMethodInfo> fastMethodInfos = handlerMethods.Select(x => new FastMethodInfo(x));

                handlerDescriptors.Add(new HandlerDescriptor(handlerType, fastMethodInfos));
            }

            return handlerDescriptors;
        }

        private static FastMethodInfo _matchHandlerMethodWithCallbackQueries(FastMethodInfo fastMethodInfo, Update update)
        {
            // TODO move this matching to other class
            // Matching for message text regex have the most priority than the others
            if (fastMethodInfo.GetMethodInfo().GetCustomAttributes(true).Any(y => y.GetType() == typeof(CallbackQueryHandler)))
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
                    if (fastMethodInfo.GetMethodInfo().GetCustomAttributes(true).Any(y => y.GetType() == typeof(UpdateTypeFilter)))
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

        public async static Task InvokeUpdate(ITelegramBotClient botClient, Update update, IEnumerable<HandlerDescriptor> handlerDescriptors)
        {
            foreach (var handlerDescriptor in handlerDescriptors)
            {
                // Find method in handler
                IEnumerable<FastMethodInfo> handlerMethodInfos = handlerDescriptor.GetMethodInfos();
                FastMethodInfo fastMethodInfo = MatchHandlerMethod(handlerMethodInfos, update);

                // Invoke first matched method
                if (fastMethodInfo != null)
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
                            var handler = scope.ResolveNamed(serviceName: handlerDescriptor.GetHandlerType().Name, handlerDescriptor.GetHandlerType());

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
}
