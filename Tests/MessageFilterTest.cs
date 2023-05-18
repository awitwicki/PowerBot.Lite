using PowerBot.Lite.HandlerInvokers;
using System.Linq;
using Tests.InitialData;
using Xunit;

namespace Tests
{
    public class MessageFilterTests
    {
        [Fact]
        public void TestStartMessageFilter()
        {
            var handlerDescriptors = MessageInvoker.CollectHandlers();
            
            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateStart);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.Start));
        }

        [Fact]
        public void TestTestMessageFilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateTest);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.Test));
        }

        [Fact]
        public void TestChatMembersAddedMessageFilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateChatMembersAdded);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateChatMembersAdded));
        }

        [Fact]
        public void TestChatMemberLeftMessageFilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateChatMembersLeft);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateChatMemberLeft));
        }

        [Fact]
        public void TestAllUpdateCallbackQueryMessageFilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateCallbackQuery);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateCallbackQueryFirst));
        }
    }
}
