using System;

namespace PowerBot.Lite.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HandlerFilterAttribute<T> : Attribute where T : IHandlerFilter
    {
        public Type FilterType => typeof(T);
    }
}
