using System;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Attributes
{
    public class MessageReaction : Attribute
    {
        public ChatAction ChatAction { get; set; }
        public MessageReaction(ChatAction chatAction)
        {
            this.ChatAction = chatAction;
        }
    }
}
