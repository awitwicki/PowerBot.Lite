using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using PowerBot.Lite.Attributes;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.HandlerInvokers
{
    internal class FastMethodInfo
    {
        private readonly Func<object, Task> _taskInvoker;

        private readonly MethodInfo MethodInfo;
        public MethodInfo GetMethodInfo() => MethodInfo;

        private readonly Type HandlerType;
        public Type GetHandlerType() => HandlerType;

        // Cached attribute data
        public string MessageHandlerPattern { get; }
        public string CallbackQueryHandlerPattern { get; }
        public MessageType? MessageTypeFilter { get; }
        public UpdateType? UpdateTypeFilter { get; }
        public ChatAction? ChatAction { get; }
        public IReadOnlyList<Type> HandlerFilterTypes { get; }

        public FastMethodInfo(MethodInfo methodInfo, Type handlerType)
        {
            HandlerType = handlerType;
            MethodInfo = methodInfo;

            // Cache all attribute data at construction time
            var attributes = methodInfo.GetCustomAttributes(true);

            MessageHandlerPattern = attributes
                .OfType<MessageHandlerAttribute>()
                .FirstOrDefault()?.Pattern;

            CallbackQueryHandlerPattern = attributes
                .OfType<CallbackQueryHandlerAttribute>()
                .FirstOrDefault()?.DataPattern;

            MessageTypeFilter = attributes
                .OfType<MessageTypeFilterAttribute>()
                .FirstOrDefault()?.MessageType;

            UpdateTypeFilter = attributes
                .OfType<UpdateTypeFilterAttribute>()
                .FirstOrDefault()?.UpdateType;

            ChatAction = attributes
                .OfType<MessageReactionAttribute>()
                .FirstOrDefault()?.ChatAction;

            HandlerFilterTypes = attributes
                .Where(a => a.GetType().IsGenericType &&
                            a.GetType().GetGenericTypeDefinition() == typeof(HandlerFilterAttribute<>))
                .Select(a => a.GetType().GetGenericArguments()[0])
                .ToArray();

            // Compile fast invoker
            var instanceExpression = Expression.Parameter(typeof(object), "instance");
            var instanceCast = !methodInfo.IsStatic
                ? Expression.Convert(instanceExpression, methodInfo.ReflectedType!)
                : null;

            var callExpression = Expression.Call(instanceCast, methodInfo);

                // Upcast Task<T> to Task so we can await uniformly.
                var asTaskExpression = Expression.Convert(callExpression, typeof(Task));
                _taskInvoker = Expression
                    .Lambda<Func<object, Task>>(asTaskExpression, instanceExpression)
                    .Compile();
        }

        public Task InvokeAsync(object instance)
        {
            return _taskInvoker(instance);
        }
    }
}
