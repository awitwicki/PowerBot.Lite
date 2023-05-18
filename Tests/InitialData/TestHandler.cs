using System.Threading.Tasks;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types.Enums;

namespace Tests.InitialData;

internal class TestHandler : BaseHandler
{
    [MessageHandler("/start")]
    public Task Start() 
    {
        return Task.CompletedTask;
    }

    [MessageHandler("/test")]
    public Task Test()
    {
        return Task.CompletedTask;
    }

    [MessageTypeFilter(MessageType.ChatMembersAdded)]
    public Task UpdateChatMembersAdded()
    {
        return Task.CompletedTask;
    }

    [MessageTypeFilter(MessageType.ChatMemberLeft)]
    public Task UpdateChatMemberLeft()
    {
        return Task.CompletedTask;
    }

    [CallbackQueryHandlerAttribute("bbb")]
    public Task UpdateCallbackQueryFirst()
    {
        return Task.CompletedTask;
    }

    [UpdateTypeFilterAttribute(UpdateType.CallbackQuery)]
    public Task UpdateCallbackQuerySecond()
    {
        return Task.CompletedTask;
    }
}
