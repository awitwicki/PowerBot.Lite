using Autofac;
using Autofac.Builder;
using Autofac.Features.AttributeFilters;
using PowerBot.Lite.HandlerInvokers;
using PowerBot.Lite.Handlers;
using PowerBot.Lite.Middlewares;
using PowerBot.Lite.Services;
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
        private ContainerBuilder _containerBuilder { get; set; }
        private List<HandlerDescriptor> _handlerDescriptors { get; set; }
        public CoreBot(string botToken)
        {
            // Create DI container
            _containerBuilder = new ContainerBuilder();

            botClient = new TelegramBotClient(botToken);
        }

        public ContainerBuilder ContainerBuilder => _containerBuilder;

        public CoreBot RegisterContainers(Action<ContainerBuilder> action)
        {
            // Register containers from client app
            action.Invoke(_containerBuilder);

            return this;
        }

        public CoreBot Build()
        {
            // Get all middlewares
            var middlewares = ReflectiveEnumerator.GetEnumerableOfType<BaseMiddleware>();

            // Register middlewares
            foreach (var middleware in middlewares)
            {
                _containerBuilder.RegisterType(middleware)
                    .As<IBaseMiddleware>()
                    .InstancePerLifetimeScope()
                    .WithAttributeFiltering();
            }

            // Get all handler descriptors
            _handlerDescriptors = MessageInvoker.CollectHandlers();

            // Register handlers
            foreach (var handlerMethodType in _handlerDescriptors)
            {
                _containerBuilder.RegisterType(handlerMethodType.GetHandlerType())
                    .Named(handlerMethodType.GetHandlerType().Name, handlerMethodType.GetHandlerType())
                    .InstancePerLifetimeScope()
                    .WithAttributeFiltering();
            }

            // Build container
            DIContainerInstance.Container = _containerBuilder.Build();

            return this;
        }

        public async Task StartReveiving()
        {
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

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // Get all methods to run to list
                IEnumerable<FastMethodInfo> filteredFastMethods = MessageInvoker.FilterFastMethods(update, _handlerDescriptors);

                // Handle message delegate
                var processMethodsFunc = async () => await MessageInvoker.InvokeUpdate(botClient, update, filteredFastMethods);

                // Invoke middleware with message processing delegate
                // Not await. Do not wait until Invoke will be finished
                _ = MiddlewareInvoker.InvokeUpdate(botClient, update, processMethodsFunc);
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
