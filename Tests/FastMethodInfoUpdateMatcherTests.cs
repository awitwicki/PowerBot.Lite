using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PowerBot.Lite.HandlerInvokers;
using Telegram.Bot.Types;
using Tests.InitialData;
using Xunit;

namespace Tests;

public class FastMethodInfoUpdateMatcherTests
{
    [Theory]
    [ClassData(typeof(UpdateWithComparedHandlerMethodList))]
    public void MatchHandlerMethod_WithValidInput_ReturnsExpectedMethodInfo(Update update, Type handlerType,
        MethodInfo expectedMethodInfo)
    {
        var handlerTypes = new List<Type> { handlerType };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        var matchedHandlerMethod = FastMethodInfoUpdateMatcher
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), update);
        
        Assert.Equal(expectedMethodInfo, matchedHandlerMethod.GetMethodInfo());
    }
    
    [Theory]
    [ClassData(typeof(UpdateWithComparedHandlersMethodList))]
    public void MatchHandlerMethod_WithValidInputHandlers_ReturnsExpectedMethodInfo(Update update,
        List<Type> handlerTypes,
        MethodInfo expectedMethodInfo)
    {
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        var matchedHandlerMethod = FastMethodInfoUpdateMatcher
            .MatchHandlerMethod(handlerDescriptors[0].GetMethodInfos(), update);
        
        Assert.Equal(expectedMethodInfo, matchedHandlerMethod.GetMethodInfo());
    }
}
