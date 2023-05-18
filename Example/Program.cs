using Autofac;
using Example;
using Example.Middlewares;
using Example.Services;
using PowerBot.Lite;

Console.WriteLine("Hello, World!");
Console.WriteLine("Starting PowerBot.Lite");

var botToken = "";

if (botToken == null)
{
    throw new Exception("ENV PowerBot.Lite_TELEGRAM_TOKEN is not defined");
}

var botClient = new CoreBot(botToken)
    .RegisterContainers(x => {
        x.RegisterType<RandomService>()
            .As<IRandomService>()
            .SingleInstance();
    })
    .RegisterMiddleware<BotMiddleware>()
    .RegisterMiddleware<BotSecondMiddleware>()
    .RegisterHandler<BotHandler>()
    .Build();

    await botClient.StartReveiving();

// Wait for eternity
await Task.Delay(Int32.MaxValue);
