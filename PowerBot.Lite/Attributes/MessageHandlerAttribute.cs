using System;

namespace PowerBot.Lite.Attributes
{
    public class MessageHandlerAttribute : Attribute
    {
        public string Pattern { get; set; }

        public MessageHandlerAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
