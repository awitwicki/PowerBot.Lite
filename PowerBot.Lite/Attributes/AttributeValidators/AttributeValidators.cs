using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.Attributes.AttributeValidators
{
    internal class AttributeValidators
    {
        protected AttributeValidators() { }

        // Send chat action
        public static Nullable<ChatAction> GetChatActionAttributes(MethodInfo methodInfo)
        {
            Object[] attributes = methodInfo.GetCustomAttributes(true);

            //find and return chatAction
            foreach (var attribute in attributes)
            {
                if (attribute.GetType() == typeof(MessageReaction))
                {
                    var chatAction = ((MessageReaction)attribute).ChatAction;
                    return chatAction;
                }
            }

            return null;
        }

        // TODO merge 4 this methods to one "universal"
        // Match message text with method
        public static bool MatchMessageHandlerMethod(MethodInfo methodInfo, string inputText)
        {
            Object[] attributes = methodInfo.GetCustomAttributes(true);

            //find and return chatAction
            foreach (var attribute in attributes)
            {
                if (attribute.GetType() == typeof(MessageHandler))
                {
                    var pattern = ((MessageHandler)attribute).Pattern;

                    //regex match
                    Match m = Regex.Match(inputText, pattern, RegexOptions.IgnoreCase);
                    return m.Success;
                }
            }

            return false;
        }

        // Match callbackQuery data with method
        public static bool MatchCallbackQueryHandlerMethod(MethodInfo methodInfo, string inputText)
        {
            Object[] attributes = methodInfo.GetCustomAttributes(true);

            //find and return chatAction
            foreach (var attribute in attributes)
            {
                if (attribute.GetType() == typeof(CallbackQueryHandlerAttribute))
                {
                    var pattern = ((CallbackQueryHandlerAttribute)attribute).DataPattern;

                    //regex match
                    Match m = Regex.Match(inputText, pattern, RegexOptions.IgnoreCase);
                    return m.Success;
                }
            }

            return false;
        }

        // Match update type
        public static bool MatchUpdateType(MethodInfo methodInfo, Update update)
        {
            Object[] attributes = methodInfo.GetCustomAttributes(true);

            UpdateTypeFilterAttribute updateTypeFilter = attributes
                .Where(attribute => attribute.GetType() == typeof(UpdateTypeFilterAttribute))
                .Cast<UpdateTypeFilterAttribute>()
                .FirstOrDefault();

            if (updateTypeFilter != null)
            {
                return updateTypeFilter.UpdateType == update.Type;
            }

            return true;
        }

        // Match message type
        public static bool MatchMessageType(MethodInfo methodInfo, Update update)
        {
            Object[] attributes = methodInfo.GetCustomAttributes(true);

            MessageTypeFilter updateTypeFilter = attributes
                .Where(attribute => attribute.GetType() == typeof(MessageTypeFilter))
                .Cast<MessageTypeFilter>()
                .FirstOrDefault();

            if (updateTypeFilter != null)
            {
                return updateTypeFilter.MessageType == update.Message.Type;
            }

            return true;
        }
    }
}
