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
    public static Update UpdateCallbackQueryBbb => new()
    {
        CallbackQuery = new CallbackQuery
        {
            Data = "bbb"
        }
    };
    public static Update UpdateCallbackQueryLorem => new()
    {
        CallbackQuery = new CallbackQuery
        {
            Data = "lorem"
        }
    };
    public static Update UpdateRandomCallbackQueryLoremIpsum => new()
    {
        CallbackQuery = new CallbackQuery
        {
            Data = "lorem_ipsum"
        }
    };
    public static Update UpdateRandomCallbackQueryGlobal => new()
    {
        CallbackQuery = new CallbackQuery
        {
            Data = "Global"
        }
    };
}
