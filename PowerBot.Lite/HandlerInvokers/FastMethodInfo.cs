using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PowerBot.Lite.HandlerInvokers
{
    public class FastMethodInfo
    {
        private delegate void TaskDelegate(object instance);
        private TaskDelegate Delegate { get; }
        private MethodInfo MethodInfo { get; set; }
        public MethodInfo GetMethodInfo() => MethodInfo;
        private Type HandlerType { get; set; }
        public Type GetHandlerType() => HandlerType;
        public FastMethodInfo(MethodInfo methodInfo, Type handlerType)
        {
            HandlerType = handlerType;
            MethodInfo = methodInfo;
            var instanceExpression = Expression.Parameter(typeof(object), "instance");

            var callExpression = Expression.Call(
                !methodInfo.IsStatic ? Expression.Convert(instanceExpression, methodInfo.ReflectedType!) : null,
                methodInfo);

            var taskDelegate = Expression
                .Lambda<TaskDelegate>(callExpression, instanceExpression)
                .Compile();

            Delegate = (instance) =>
            {
                taskDelegate(instance);
            };
        }

        public void Invoke(object instance)
        {
            Delegate(instance);
        }
    }
}
