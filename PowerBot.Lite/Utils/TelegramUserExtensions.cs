using Telegram.Bot.Types;

namespace PowerBot.Lite.Utils
{
    public static class TelegramUserExtensions
    {
        public static string GetUserMention(this User user)
        {
            if (user.Username != null)
            {
                return "@" + user.Username.Replace("_", "\\_");
            }
            else
            {
                return $"[{user.FirstName}](tg://user?id={user.Id})";
            }
        }
    }
}
