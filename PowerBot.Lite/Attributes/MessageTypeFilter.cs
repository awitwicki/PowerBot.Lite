using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Attributes
{
    public class MessageTypeFilter : Attribute
    { 
        public MessageType MessageType { get; set; }
        public MessageTypeFilter(MessageType messageType)
        {
            this.MessageType = messageType;
        }
    }
}
