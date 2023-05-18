using Telegram.Bot.Types;

namespace Tests.InitialData;

public static class UpdateBuilder
{
    public static Update UpdateStart => new()
    {
        Message = new Message
        {
            Text = "/start"
        },
    };
    public static Update UpdateTest => new()
    {
        Message = new Message
        {
            Text = "/test"
        }
    };
    public static Update UpdateChatMembersAdded => new()
    {
        Message = new Message
        {
            NewChatMembers = new User[] { new() { Id = 234234 } }
        }
    };
    public static Update UpdateChatMembersLeft => new()
    {
        Message = new Message
        {
            LeftChatMember = new User { Id = 234234 } 
        }
    };
    public static Update UpdateCallbackQuery => new()
    {
        CallbackQuery = new CallbackQuery
        {
            Data = "bbb"
        }
    };
    public static Update UpdateRandomCallbackQuery => new()
    {
        CallbackQuery = new CallbackQuery
        {
            Data = "loorem ipsum"
        }
    };
}
