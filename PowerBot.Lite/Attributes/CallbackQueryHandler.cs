using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBot.Lite.Attributes
{
    public class CallbackQueryHandler : Attribute
    {
        public string DataPattern { get; set; }

        public CallbackQueryHandler(string dataPattern = null)
        {
            if (dataPattern != null)
            {
                this.DataPattern = dataPattern;
            }
        }
    }
}
