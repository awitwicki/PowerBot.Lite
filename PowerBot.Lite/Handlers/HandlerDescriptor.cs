using PowerBot.Lite.HandlerInvokers;
using System;
using System.Collections.Generic;

namespace PowerBot.Lite.Handlers
{
    public class HandlerDescriptor
    {
        private Type HandlerType { get; set; }
        public Type GetHandlerType() => HandlerType;
        private IEnumerable<FastMethodInfo> MethodInfos { get; set; }
        public IEnumerable<FastMethodInfo> GetMethodInfos() => MethodInfos;

        public HandlerDescriptor(Type handlerType, IEnumerable<FastMethodInfo> methodInfos)
        {
            HandlerType = handlerType;
            MethodInfos = methodInfos;
        }
    }
}
