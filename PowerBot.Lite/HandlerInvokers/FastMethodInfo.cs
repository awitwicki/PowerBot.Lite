using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerBot.Lite.HandlerInvokers
{
    public class FastMethodInfo
    {

        private delegate void TaskDelegate(object instance);
        private TaskDelegate Delegate { get; }
        private MethodInfo _methodInfo { get; set; }
        public MethodInfo GetMethodInfo() => _methodInfo;
        public FastMethodInfo(MethodInfo methodInfo)
        {
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
