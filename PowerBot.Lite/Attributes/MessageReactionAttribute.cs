using System;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Attributes
{
    public class MessageReactionAttribute : Attribute
    {
        public ChatAction ChatAction { get; set; }
        public MessageReactionAttribute(ChatAction chatAction)
        {
            ChatAction = chatAction;
        }
    }
}
