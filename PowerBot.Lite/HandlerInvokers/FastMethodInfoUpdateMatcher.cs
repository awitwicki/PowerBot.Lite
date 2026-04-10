using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PowerBot.Lite.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite.HandlerInvokers;

internal static class FastMethodInfoUpdateMatcher
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
        if (fastMethodInfo.CallbackQueryHandlerPattern != null &&
            Regex.Match(update.CallbackQuery!.Data, fastMethodInfo.CallbackQueryHandlerPattern, RegexOptions.IgnoreCase).Success)
        {
            return fastMethodInfo;
        }

        return null;
    }

    private static FastMethodInfo _matchHandlerMethodWithMessages(FastMethodInfo fastMethodInfo, Update update)
    {
        // Matching for message text regex have the most priority than the others
        if (update?.Message?.Text != null &&
            fastMethodInfo.MessageHandlerPattern != null &&
            Regex.Match(update.Message.Text, fastMethodInfo.MessageHandlerPattern, RegexOptions.IgnoreCase).Success)
        {
            return fastMethodInfo;
        }

        // Message type
        if (update?.Type == UpdateType.Message &&
            fastMethodInfo.MessageTypeFilter.HasValue &&
            fastMethodInfo.MessageTypeFilter.Value == update.Message.Type)
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
            if (matchedMethod == null && fastMethodInfo.UpdateTypeFilter.HasValue)
            {
                if (fastMethodInfo.UpdateTypeFilter.Value != update.Type)
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
