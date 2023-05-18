using System.Threading.Tasks;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types.Enums;

namespace Tests.InitialData;

internal class TestHandler2 : BaseHandler
{
    [CallbackQueryHandler("bbb")]
    public Task UpdateCallbackQueryFirst()
    {
        return Task.CompletedTask;
    }

    [UpdateTypeFilter(UpdateType.CallbackQuery)]
    public Task UpdateCallbackQuerySecond()
    {
        return Task.CompletedTask;
    }
}
