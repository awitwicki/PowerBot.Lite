using System.Threading.Tasks;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types;

namespace PowerBot.Lite.Tests.InitialData;

internal class TestAllowFilter : IHandlerFilter
{
    public Task<bool> FilterAsync(Update update) => Task.FromResult(true);
}

internal class TestDenyFilter : IHandlerFilter
{
    public Task<bool> FilterAsync(Update update) => Task.FromResult(false);
}

internal class TestFilterHandler : BaseHandler
{
    [HandlerFilter<TestAllowFilter>]
    [MessageHandler("/filtered")]
    public Task FilteredMethod() => Task.CompletedTask;

    [HandlerFilter<TestAllowFilter>]
    [HandlerFilter<TestDenyFilter>]
    [MessageHandler("/multifiltered")]
    public Task MultiFilteredMethod() => Task.CompletedTask;
}
