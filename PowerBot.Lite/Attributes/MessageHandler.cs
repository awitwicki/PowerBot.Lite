﻿using System;

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
