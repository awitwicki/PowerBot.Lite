using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace PowerBot.Lite.Attributes
{
    public interface IHandlerFilter
    {
        Task<bool> FilterAsync(Update update);
    }
}
