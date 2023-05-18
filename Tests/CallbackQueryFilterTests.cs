using System.Linq;
using PowerBot.Lite.HandlerInvokers;
using Tests.InitialData;
using Xunit;

namespace Tests;

public class CallbackQueryFilterTest
{
    [Fact]
    public void TestCallbackQueryWithSpecifiedData()
    {
        // Create mock update type
        var handlerDescriptors = MessageInvoker.CollectHandlers();

        FastMethodInfo matchedHandlerMethod = MessageInvoker
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler2.UpdateCallbackQueryFirst));
    }

    [Fact]
    public void TestCallbackQueryType()
    {
        // Create mock update type
        var handlerDescriptors = MessageInvoker.CollectHandlers();

        FastMethodInfo matchedHandlerMethod = MessageInvoker
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateRandomCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler2.UpdateCallbackQuerySecond));
    }

    [Fact]
    public void TestCallbackQueryWithSpecifiedDataAll()
    {
        // Create mock update type
        var handlerDescriptors = MessageInvoker.CollectHandlers();

        FastMethodInfo matchedHandlerMethod = MessageInvoker
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateCallbackQueryFirst));
    }

    [Fact]
    public void TestCallbackQueryTypeAll()
    {
        //ITelegramBotClient telegramBotClient;// = new TelegramBotClient();
        // Create mock update type

        var handlerDescriptors = MessageInvoker.CollectHandlers();

        FastMethodInfo matchedHandlerMethod = MessageInvoker
            .MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateRandomCallbackQuery);

        Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateCallbackQuerySecond));
    }
}
