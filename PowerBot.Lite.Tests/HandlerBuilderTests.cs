﻿using System;
using System.Collections.Generic;
using System.Linq;
using PowerBot.Lite.HandlerInvokers;
using PowerBot.Lite.Tests.InitialData;
using Xunit;

namespace PowerBot.Lite.Tests;

public class HandlerBuilderTests
{
    [Fact]
    public void BuildHandlerDescriptor_WithTestHandler_ReturnsTestHandlerDescriptor()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToList();

        Assert.Single(handlerDescriptors);
        Assert.Equal(typeof(TestHandler), handlerDescriptors[0].GetHandlerType());
    }
    
    [Theory]
    [InlineData(typeof(TestHandler), 7)]
    [InlineData(typeof(TestHandler2), 3)]
    public void BuildHandlerDescriptor_WithTestHandler_ShouldContainExactCountOfFastMethodInfos(Type handlerType, int expectedMethodCount)
    {
        var handlerDescriptors = HandlerBuilder
            .BuildHandlerDescriptors(new List<Type> { handlerType })
            .ToArray();

        Assert.Equal(expectedMethodCount, handlerDescriptors[0].GetMethodInfos().Count());
    }
    
    [Fact]
    public void BuildHandlerDescriptors_WithTestHandlers_ReturnsTestHandlerDescriptors()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler), typeof(TestHandler2) };
        var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
            .ToArray();

        Assert.Equal(2, handlerDescriptors.Length);
        Assert.Equal(typeof(TestHandler), handlerDescriptors[0].GetHandlerType());
        Assert.Equal(typeof(TestHandler2),handlerDescriptors.Last().GetHandlerType());
    }
    
    [Fact]
    public void BuildHandlerDescriptors_WithInvalidType_ThrowsArgumentException()
    {
        var handlerTypes = new List<Type> { typeof(TestHandler), typeof(string) };

        Assert.Throws<ArgumentException>(() => HandlerBuilder.BuildHandlerDescriptors(handlerTypes));
    }
}
