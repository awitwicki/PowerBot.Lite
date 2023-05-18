using System;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Attributes
{
    public class MessageTypeFilterAttribute : Attribute
    { 
        public MessageType MessageType { get; set; }
        public MessageTypeFilterAttribute(MessageType messageType)
        {
            MessageType = messageType;
        }
    }
}
