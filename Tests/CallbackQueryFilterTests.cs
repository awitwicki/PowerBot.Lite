using System;
using System.Collections.Generic;
using System.Linq;
using PowerBot.Lite.HandlerInvokers;
using Tests.InitialData;
using Xunit;

namespace Tests;

// TODO fix naming
public class CallbackQueryFilterTest
{
    [Fact]
    public void TestCallbackQueryWithSpecifiedData()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        var matchedHandlerMethod = FastMethodInfoUpdateMatcher
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler2.UpdateCallbackQueryFirst));
    }

    [Fact]
    public void TestCallbackQueryType()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        var matchedHandlerMethod = FastMethodInfoUpdateMatcher
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateRandomCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler2.UpdateCallbackQuerySecond));
    }

    [Fact]
    public void TestCallbackQueryWithSpecifiedDataAll()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        var matchedHandlerMethod = FastMethodInfoUpdateMatcher
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateCallbackQueryFirst));
    }

    [Fact]
    public void TestCallbackQueryTypeAll()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        var matchedHandlerMethod = FastMethodInfoUpdateMatcher
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateRandomCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateCallbackQuerySecond));
    }
}
