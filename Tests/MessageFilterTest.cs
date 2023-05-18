using System;
using System.Collections.Generic;
using PowerBot.Lite.HandlerInvokers;
using System.Linq;
using Tests.InitialData;
using Xunit;

namespace Tests
{
    // TODO fix naming
    public class MessageFilterTests
    {
        [Fact]
        public void TestStartMessageFilter()
        {
            var handlerTypes = new List<Type> { typeof(TestHandler) };
            var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
                .ToArray();

            var matchedHandlerMethod = FastMethodInfoUpdateMatcher.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateStart);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.Start));
        }

        [Fact]
        public void TestTestMessageFilter()
        {
            var handlerTypes = new List<Type> { typeof(TestHandler) };
            var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
                .ToArray();
            
            var matchedHandlerMethod = FastMethodInfoUpdateMatcher.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateTest);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.Test));
        }

        [Fact]
        public void TestChatMembersAddedMessageFilter()
        {
            var handlerTypes = new List<Type> { typeof(TestHandler) };
            var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
                .ToArray();

            var matchedHandlerMethod = FastMethodInfoUpdateMatcher.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateChatMembersAdded);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateChatMembersAdded));
        }

        [Fact]
        public void TestChatMemberLeftMessageFilter()
        {
            var handlerTypes = new List<Type> { typeof(TestHandler) };
            var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
                .ToArray();
            
            var matchedHandlerMethod = FastMethodInfoUpdateMatcher.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateChatMembersLeft);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateChatMemberLeft));
        }

        [Fact]
        public void TestAllUpdateCallbackQueryMessageFilter()
        {
            var handlerTypes = new List<Type> { typeof(TestHandler) };
            var handlerDescriptors = HandlerBuilder.BuildHandlerDescriptors(handlerTypes)
                .ToArray();

            var matchedHandlerMethod = FastMethodInfoUpdateMatcher.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.UpdateCallbackQuery);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.UpdateCallbackQueryFirst));
        }
    }
}
