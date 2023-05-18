using System;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Attributes
{
    public class UpdateTypeFilterAttribute : Attribute
    {
        public UpdateType UpdateType { get; set; }
        public UpdateTypeFilterAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}
