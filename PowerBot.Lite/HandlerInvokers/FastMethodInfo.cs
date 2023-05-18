using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PowerBot.Lite.HandlerInvokers
{
    public class FastMethodInfo
    {

        private delegate void TaskDelegate(object instance);
        private TaskDelegate Delegate { get; }
        private MethodInfo _methodInfo { get; set; }
        public MethodInfo GetMethodInfo() => _methodInfo;
        private Type _handlerType { get; set; }
        public Type GetHandlerType() => _handlerType;
        public FastMethodInfo(MethodInfo methodInfo, Type handlerType)
        {
            _handlerType = handlerType;
            _methodInfo = methodInfo;
            var instanceExpression = Expression.Parameter(typeof(object), "instance");

            var callExpression = Expression.Call(!methodInfo.IsStatic ? Expression.Convert(instanceExpression, methodInfo.ReflectedType) : null, methodInfo);

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
