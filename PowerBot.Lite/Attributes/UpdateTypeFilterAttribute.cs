using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
