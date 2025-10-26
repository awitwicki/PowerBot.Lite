using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PowerBot.Lite.HandlerInvokers
{
    internal class FastMethodInfo
    {
        private readonly Func<object, Task> _taskInvoker;

        private readonly MethodInfo MethodInfo;
        public MethodInfo GetMethodInfo() => MethodInfo;

        private readonly Type HandlerType;
        public Type GetHandlerType() => HandlerType;

        public FastMethodInfo(MethodInfo methodInfo, Type handlerType)
        {
            HandlerType = handlerType;
            MethodInfo = methodInfo;

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
