using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBot.Lite.Attributes
{
    public class MessageHandler : Attribute
    {
        public string Pattern { get; set; }

        public MessageHandler(string pattern)
        {
            this.Pattern = pattern;
        }
    }
}
