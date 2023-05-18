using System;

namespace PowerBot.Lite.Attributes
{
    public class CallbackQueryHandlerAttribute : Attribute
    {
        public string DataPattern { get; set; }

        public CallbackQueryHandlerAttribute(string dataPattern = null)
        {
            if (dataPattern != null)
            {
                DataPattern = dataPattern;
            }
        }
    }
}
