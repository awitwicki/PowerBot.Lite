using System;
using PowerBot.Lite;
using PowerBot.Lite.Exceptions;
using PowerBot.Lite.Tests.InitialData;
using Xunit;

namespace PowerBot.Lite.Tests;

public class CoreBotTests
{
    const string Token = "TELEGRAM_BOT_TOKEN";

    [Fact] public void CoreBot_WithValidHandler_BuildsSuccessfully()
    {
        // Arrange
        var coreBot = new CoreBot(Token)
            .RegisterHandler<TestHandler>();
           
        // Act and Assert
        coreBot.Build();
        
        // Assert
        Assert.True(true);
    }
    
    [Fact]
    public void CoreBot_WithValidTestMiddleware_BuildsSuccessfully()
    {
        // Arrange
        var coreBot = new CoreBot(Token)
            .RegisterMiddleware<TestMiddleware>();
           
        // Act and Assert
        coreBot.Build();
        
        // Assert
        Assert.True(true);
    }
    
    [Fact]
    public void CoreBot_WithValidTestMiddlewareAndHandler_BuildsSuccessfully()
    {
        // Arrange
        var coreBot = new CoreBot(Token)
            .RegisterHandler<TestHandler>()
            .RegisterMiddleware<TestMiddleware>();
           
        // Act and Assert
        coreBot.Build();
        
        // Assert
        Assert.True(true);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CoreBot_WithNullToken_ThrowsArgumentNullException(string token)
    {
        // Arrange and Act and Assert
        Assert.Throws<ArgumentNullException>(() => new CoreBot(token).RegisterHandler<TestHandler>());
    }

    [Fact]
    public void CoreBot_WithoutHandlersAndMiddlewares_ThrowsNotDefinedAnyHandlersOrMiddlewaresException()
    {
        // Arrange
        var coreBot = new CoreBot(Token);
           
        // Act and Assert
        Assert.Throws<NotDefinedAnyHandlersOrMiddlewaresException>(() => coreBot.Build());
    }
}
