using Autofac;
using Example.Services;
using PowerBot.Lite;

Console.WriteLine("Hello, World!");
Console.WriteLine("Starting PowerBot.Lite");

string botToken = "";

if (botToken == null)
{
    throw new Exception("ENV PowerBot.Lite_TELEGRAM_TOKEN is not defined");
}

CoreBot botClient = new CoreBot(botToken)
    .RegisterContainers(x => {
        x.RegisterType<RandomService>()
            .As<IRandomService>()
            .SingleInstance();
    })
    .Build();

    botClient.StartReveiving();

// Wait for eternity
await Task.Delay(Int32.MaxValue);
