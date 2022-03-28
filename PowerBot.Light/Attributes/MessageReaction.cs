using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
