using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.HandlerInvokers;
using PowerBot.Lite.Handlers;
using PowerBot.Lite.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Xunit;

namespace PowerBot.Lite.Tests;

internal class InvocationTracker
{
    public bool WasInvoked { get; set; }
}

internal class TrackingAllowFilter : IHandlerFilter
{
    public static bool WasCalled { get; set; }
    public Task<bool> FilterAsync(Update update)
    {
        WasCalled = true;
        return Task.FromResult(true);
    }
}

internal class TrackingDenyFilter : IHandlerFilter
{
    public static bool WasCalled { get; set; }
    public Task<bool> FilterAsync(Update update)
    {
        WasCalled = true;
        return Task.FromResult(false);
    }
}

internal class FilterTestHandler : BaseHandler
{
    public static InvocationTracker Tracker = new();

    [HandlerFilter<TrackingAllowFilter>]
    [MessageHandler("/allowed")]
    public Task AllowedMethod()
    {
        Tracker.WasInvoked = true;
        return Task.CompletedTask;
    }

    [HandlerFilter<TrackingDenyFilter>]
    [MessageHandler("/denied")]
    public Task DeniedMethod()
    {
        Tracker.WasInvoked = true;
        return Task.CompletedTask;
    }

    [HandlerFilter<TrackingAllowFilter>]
    [HandlerFilter<TrackingDenyFilter>]
    [MessageHandler("/multifilter")]
    public Task MultiFilterMethod()
    {
        Tracker.WasInvoked = true;
        return Task.CompletedTask;
    }

    [MessageHandler("/nofilter")]
    public Task NoFilterMethod()
    {
        Tracker.WasInvoked = true;
        return Task.CompletedTask;
    }
}

internal interface ITestAuthService
{
    Task<bool> IsAuthorizedAsync(long userId);
}

internal class TestAuthService : ITestAuthService
{
    public Task<bool> IsAuthorizedAsync(long userId) => Task.FromResult(userId == 999);
}

internal class DiAwareFilter : IHandlerFilter
{
    private readonly ITestAuthService _authService;

    public DiAwareFilter(ITestAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> FilterAsync(Update update)
    {
        var userId = update.Message?.From?.Id ?? 0;
        return await _authService.IsAuthorizedAsync(userId);
    }
}

internal class DiFilterTestHandler : BaseHandler
{
    public static InvocationTracker Tracker = new();

    [HandlerFilter<DiAwareFilter>]
    [MessageHandler("/difiltered")]
    public Task DiFilteredMethod()
    {
        Tracker.WasInvoked = true;
        return Task.CompletedTask;
    }
}

public class MessageInvokerFilterTests
{
    private const string Token = "000000000:AAAA-aaaaaaaaaaaaaaaaaaaaa-aaaaaaaa";

    private (ILifetimeScope scope, ITelegramBotClient botClient) BuildScope()
    {
        var coreBot = new CoreBot(Token)
            .RegisterHandler<FilterTestHandler>();
        coreBot.Build();

        var scope = DIContainerInstance.Container.BeginLifetimeScope();
        return (scope, coreBot.BotClient);
    }

    private IEnumerable<FastMethodInfo> GetMatchingMethods(Update update)
    {
        var handlerTypes = new List<System.Type> { typeof(FilterTestHandler) };
        var descriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes);
        return FastMethodInfoUpdateMatcher.FilterFastMethods(update, descriptors);
    }

    [Fact]
    public async Task InvokeUpdate_WithAllowFilter_ExecutesMethod()
    {
        TrackingAllowFilter.WasCalled = false;
        var (scope, botClient) = BuildScope();
        FilterTestHandler.Tracker = new InvocationTracker();
        var update = new Update
        {
            Message = new Message
            {
                Text = "/allowed",
                Chat = new Chat { Id = 123 }
            }
        };
        var methods = GetMatchingMethods(update);
        await MessageInvoker.InvokeUpdate(botClient, update, methods, scope);
        Assert.True(FilterTestHandler.Tracker.WasInvoked);
        Assert.True(TrackingAllowFilter.WasCalled);
    }

    [Fact]
    public async Task InvokeUpdate_WithDenyFilter_SkipsMethod()
    {
        TrackingDenyFilter.WasCalled = false;
        var (scope, botClient) = BuildScope();
        FilterTestHandler.Tracker = new InvocationTracker();
        var update = new Update
        {
            Message = new Message
            {
                Text = "/denied",
                Chat = new Chat { Id = 123 }
            }
        };
        var methods = GetMatchingMethods(update);
        await MessageInvoker.InvokeUpdate(botClient, update, methods, scope);
        Assert.False(FilterTestHandler.Tracker.WasInvoked);
        Assert.True(TrackingDenyFilter.WasCalled);
    }

    [Fact]
    public async Task InvokeUpdate_WithMultipleFiltersOneFails_SkipsMethod()
    {
        TrackingAllowFilter.WasCalled = false;
        TrackingDenyFilter.WasCalled = false;
        var (scope, botClient) = BuildScope();
        FilterTestHandler.Tracker = new InvocationTracker();
        var update = new Update
        {
            Message = new Message
            {
                Text = "/multifilter",
                Chat = new Chat { Id = 123 }
            }
        };
        var methods = GetMatchingMethods(update);
        await MessageInvoker.InvokeUpdate(botClient, update, methods, scope);
        Assert.False(FilterTestHandler.Tracker.WasInvoked);
        Assert.True(TrackingAllowFilter.WasCalled || TrackingDenyFilter.WasCalled);
    }

    [Fact]
    public async Task InvokeUpdate_WithNoFilter_ExecutesMethod()
    {
        var (scope, botClient) = BuildScope();
        FilterTestHandler.Tracker = new InvocationTracker();
        var update = new Update
        {
            Message = new Message
            {
                Text = "/nofilter",
                Chat = new Chat { Id = 123 }
            }
        };
        var methods = GetMatchingMethods(update);
        await MessageInvoker.InvokeUpdate(botClient, update, methods, scope);
        Assert.True(FilterTestHandler.Tracker.WasInvoked);
    }

    private (ILifetimeScope scope, ITelegramBotClient botClient) BuildScopeWithDiFilter()
    {
        var coreBot = new CoreBot(Token)
            .RegisterContainers(x =>
            {
                x.RegisterType<TestAuthService>()
                    .As<ITestAuthService>()
                    .InstancePerLifetimeScope();
            })
            .RegisterHandler<DiFilterTestHandler>();
        coreBot.Build();

        var scope = DIContainerInstance.Container.BeginLifetimeScope();
        return (scope, coreBot.BotClient);
    }

    private IEnumerable<FastMethodInfo> GetMatchingMethodsForDiHandler(Update update)
    {
        var handlerTypes = new List<System.Type> { typeof(DiFilterTestHandler) };
        var descriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes);
        return FastMethodInfoUpdateMatcher.FilterFastMethods(update, descriptors);
    }

    [Fact]
    public async Task InvokeUpdate_WithDiFilter_AuthorizedUser_ExecutesMethod()
    {
        var (scope, botClient) = BuildScopeWithDiFilter();
        DiFilterTestHandler.Tracker = new InvocationTracker();
        var update = new Update
        {
            Message = new Message
            {
                Text = "/difiltered",
                Chat = new Chat { Id = 123 },
                From = new User { Id = 999 }
            }
        };
        var methods = GetMatchingMethodsForDiHandler(update);
        await MessageInvoker.InvokeUpdate(botClient, update, methods, scope);
        Assert.True(DiFilterTestHandler.Tracker.WasInvoked);
    }

    [Fact]
    public async Task InvokeUpdate_WithDiFilter_UnauthorizedUser_SkipsMethod()
    {
        var (scope, botClient) = BuildScopeWithDiFilter();
        DiFilterTestHandler.Tracker = new InvocationTracker();
        var update = new Update
        {
            Message = new Message
            {
                Text = "/difiltered",
                Chat = new Chat { Id = 123 },
                From = new User { Id = 1 }
            }
        };
        var methods = GetMatchingMethodsForDiHandler(update);
        await MessageInvoker.InvokeUpdate(botClient, update, methods, scope);
        Assert.False(DiFilterTestHandler.Tracker.WasInvoked);
    }
}
