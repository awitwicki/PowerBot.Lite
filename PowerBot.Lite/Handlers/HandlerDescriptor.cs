using PowerBot.Lite.HandlerInvokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBot.Lite.Handlers
{
    public class HandlerDescriptor
    {
        private Type _handlerType { get; set; }
        public Type GetHandlerType() => _handlerType;
        private IEnumerable<FastMethodInfo> _methodInfos { get; set; }
        public IEnumerable<FastMethodInfo> GetMethodInfos() => _methodInfos;

        public HandlerDescriptor(Type handlerType, IEnumerable<FastMethodInfo> methodInfos)
        {
            _handlerType = handlerType;
            _methodInfos = methodInfos;
        }
    }
}
