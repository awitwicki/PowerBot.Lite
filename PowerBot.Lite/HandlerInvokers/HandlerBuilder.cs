using System;
using System.Collections.Generic;
using System.Linq;
using PowerBot.Lite.Handlers;

namespace PowerBot.Lite.HandlerInvokers;

internal static class HandlerBuilder
{
    public static IEnumerable<HandlerDescriptor> BuildHandlerDescriptors(IEnumerable<Type> handlerTypes)
    {
        return handlerTypes.Select(BuildHandlerDescriptor).ToList();
    }

    private static HandlerDescriptor BuildHandlerDescriptor(Type type)
    {
        if (!type.IsSubclassOf(typeof(BaseHandler)))
        {
            throw new ArgumentException($"Provided class {nameof(type)} must be subclass of BaseHandler");
        }

        var handlerMethods = type.GetMethods()
            .Where(x => x.DeclaringType != typeof(BaseHandler))
            .Where(x => x.DeclaringType != typeof(Object));
        
        var fastMethodInfos = handlerMethods.Select(x => new FastMethodInfo(x, type));

        return new HandlerDescriptor(type, fastMethodInfos);
    }
}
