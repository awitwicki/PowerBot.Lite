using System;
using System.Collections.Generic;
using System.Linq;
using PowerBot.Lite.HandlerInvokers;
using Tests.InitialData;
using Xunit;

namespace Tests;

public class HandlerBuilderTests
{
    [Fact]
    public void BuildHandlerDescriptor_WithTestHandler_ReturnsTestHandlerDescriptor()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        Assert.True(handlerDescriptors.Length == 1);
        Assert.True(handlerDescriptors.First().GetHandlerType() == typeof(TestHandler));
    }
    
    [Theory]
    [InlineData(typeof(TestHandler), 6)]
    [InlineData(typeof(TestHandler2), 2)]
    public void BuildHandlerDescriptor_WithTestHandler_ShouldContainExactCountOfFastMethodInfos(Type handlerType, int expectedMethodCount)
    {
        var handlerDescriptors = HandlerBuilder
            .BuildHandlerDescriptors(new List<Type> { handlerType })
            .ToArray();

        Assert.True(handlerDescriptors.First().GetMethodInfos().Count() == expectedMethodCount);
    }
    
    [Fact]
    public void BuildHandlerDescriptors_WithTestHandlers_ReturnsTestHandlerDescriptors()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler), typeof(TestHandler2) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        Assert.True(handlerDescriptors.Length == 2);
        Assert.True(handlerDescriptors.First().GetHandlerType() == typeof(TestHandler));
        Assert.True(handlerDescriptors.Last().GetHandlerType() == typeof(TestHandler2));
    }
    
    [Fact]
    public void BuildHandlerDescriptors_WithInvalidType_ThrowsArgumentException()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler), typeof(String) };

        Assert.Throws<ArgumentException>(() => HandlerBuilder.BuildHandlerDescriptors(handlerTypes));
    }
}
