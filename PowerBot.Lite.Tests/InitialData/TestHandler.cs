using System.Threading.Tasks;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Tests.InitialData;

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

    [MessageTypeFilter(MessageType.NewChatMembers)]
    public Task UpdateChatMembersAdded()
    {
        return Task.CompletedTask;
    }

    [MessageTypeFilter(MessageType.LeftChatMember)]
    public Task UpdateChatMemberLeft()
    {
        return Task.CompletedTask;
    }

    [CallbackQueryHandler("bbb")]
    public Task UpdateCallbackQueryDataBbb()
    {
        return Task.CompletedTask;
    }

    [UpdateTypeFilter(UpdateType.CallbackQuery)]
    public Task UpdateCallbackQueryGlobal()
    {
        return Task.CompletedTask;
    }
    
    [MessageTypeFilter(MessageType.Voice)]
    public Task UpdateVoice()
    {
        return Task.CompletedTask;
    }
}
