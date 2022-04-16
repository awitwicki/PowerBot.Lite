using PowerBot.Lite.HandlerInvokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PowerBot.Lite
{
    public class CoreBot
    {
        public ITelegramBotClient botClient { get; set; }

        public CoreBot(string botToken)
        {
            botClient = new TelegramBotClient(botToken);

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
                ThrowPendingUpdates = true
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions);

            var me = botClient.GetMeAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Start listening druzhokbot for @{me.Username}");
        }
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // Invoke middleware
                await MiddlewareInvoker.InvokeUpdate(botClient, update);

                // Handle message, do not wait until Invoke will be finished
                MessageInvoker.InvokeUpdate(botClient, update);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            Console.WriteLine(exception.StackTrace);

            return Task.CompletedTask;
        }
    }
}
