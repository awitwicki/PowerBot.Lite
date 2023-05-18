using System.Collections.Generic;
using System.Linq;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Attributes.AttributeValidators;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.HandlerInvokers;

public static class FastMethodInfoUpdateMatcher
{
    public static IEnumerable<FastMethodInfo> FilterFastMethods(Update update,
        IEnumerable<HandlerDescriptor> handlerDescriptors)
    {
        return handlerDescriptors
            .Select(x => MatchHandlerMethod(x.GetMethodInfos(), update))
            .Where(x => x is not null)
            .ToList();
    }

    private static FastMethodInfo _matchHandlerMethodWithCallbackQueries(FastMethodInfo fastMethodInfo, Update update)
    {
        // Matching for message text regex have the most priority than the others
        if (fastMethodInfo.GetMethodInfo().GetCustomAttributes(true)
                .Any(y => y.GetType() == typeof(CallbackQueryHandlerAttribute)) &&
            // Pattern matching for CallbackQuery data
            AttributeValidators.MatchCallbackQueryHandlerMethod(fastMethodInfo.GetMethodInfo(),
                update.CallbackQuery!.Data))
        {
            return fastMethodInfo;
        }

        return null;
    }

    private static FastMethodInfo _matchHandlerMethodWithMessages(FastMethodInfo fastMethodInfo, Update update)
    {
        // Matching for message text regex have the most priority than the others
        if (update?.Message?.Text != null && fastMethodInfo.GetMethodInfo().GetCustomAttributes(true)
                .Any(y => y.GetType() == typeof(MessageHandlerAttribute)) &&
            // Pattern matching for message text
            AttributeValidators.MatchMessageHandlerMethod(fastMethodInfo.GetMethodInfo(), update.Message.Text))
        {
            return fastMethodInfo;
        }

        // Message type
        // If method have universal update type attribute filter then check it
        if (update?.Type == UpdateType.Message && fastMethodInfo.GetMethodInfo().GetCustomAttributes(true)
                .Any(y => y.GetType() == typeof(MessageTypeFilterAttribute)) &&
            // Pattern matching for message text
            AttributeValidators.MatchMessageType(fastMethodInfo.GetMethodInfo(), update))
        {
            return fastMethodInfo;
        }

        return null;
    }

    public static FastMethodInfo MatchHandlerMethod(IEnumerable<FastMethodInfo> fastMethodInfosCollection,
        Update update)
    {
        // Filter methods attributes
        foreach (var fastMethodInfo in fastMethodInfosCollection)
        {
            var matchedMethod = update.Type switch
            {
                UpdateType.CallbackQuery => _matchHandlerMethodWithCallbackQueries(fastMethodInfo, update),
                UpdateType.Message => _matchHandlerMethodWithMessages(fastMethodInfo, update),
                _ => null
            };

            // Check for update type
            if (matchedMethod == null && fastMethodInfo.GetMethodInfo().GetCustomAttributes(true)
                    .Any(y => y.GetType() == typeof(UpdateTypeFilterAttribute)))
            {
                // Pattern matching for message text
                if (!AttributeValidators.MatchUpdateType(fastMethodInfo.GetMethodInfo(), update))
                {
                    continue;
                }

                return fastMethodInfo;
            }

            if (matchedMethod != null)
            {
                return matchedMethod;
            }
        }

        return null;
    }
}
