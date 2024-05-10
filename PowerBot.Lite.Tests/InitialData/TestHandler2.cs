using System.Threading.Tasks;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Tests.InitialData;

internal class TestHandler2 : BaseHandler
{
    [CallbackQueryHandler("lorem$")]
    public Task UpdateCallbackQueryLorem()
    {
        return Task.CompletedTask;
    }
    
    [CallbackQueryHandler("lorem_ipsum")]
    public Task UpdateCallbackQueryLoremIpsum()
    {
        return Task.CompletedTask;
    }

    [UpdateTypeFilter(UpdateType.CallbackQuery)]
    public Task UpdateCallbackQueryGlobal()
    {
        return Task.CompletedTask;
    }
}
