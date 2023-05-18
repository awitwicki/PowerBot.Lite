using PowerBot.Lite.Attributes;
using PowerBot.Lite.HandlerInvokers;
using PowerBot.Lite.Handlers;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Tests
{
    internal class TestHandler : BaseHandler
    {
        [MessageHandler("/start")]
        public Task Start() 
        {
            return Task.CompletedTask;
        }

        [MessageHandler("/test")]
        public Task test()
        {
            return Task.CompletedTask;
        }

        [MessageTypeFilter(MessageType.ChatMembersAdded)]
        public Task updateChatMembersAdded()
        {
            return Task.CompletedTask;
        }

        [MessageTypeFilter(MessageType.ChatMemberLeft)]
        public Task updateChatMemberLeft()
        {
            return Task.CompletedTask;
        }

        [CallbackQueryHandlerAttribute("bbb")]
        public Task updateCallbackQueryFirst()
        {
            return Task.CompletedTask;
        }

        [UpdateTypeFilterAttribute(UpdateType.CallbackQuery)]
        public Task updateCallbackQuerySecond()
        {
            return Task.CompletedTask;
        }
    }
    internal class TestHandler2 : BaseHandler
    {
        [CallbackQueryHandlerAttribute("bbb")]
        public Task updateCallbackQueryFirst()
        {
            return Task.CompletedTask;
        }

        [UpdateTypeFilterAttribute(UpdateType.CallbackQuery)]
        public Task updateCallbackQuerySecond()
        {
            return Task.CompletedTask;
        }
    }

    public static class UpdateBuilder
    {
        public static Update updateStart => new Update
        {
            Message = new Message
            {
                Text = "/start"
            },
        };
        public static Update updateTest => new Update
        {
            Message = new Message
            {
                Text = "/test"
            }
        };
        public static Update updateChatMembersAdded => new Update
        {
            Message = new Message
            {
                NewChatMembers = new User[] { new User { Id = 234234 } }
            }
        };
        public static Update updateChatMembersLeft => new Update
        {
            Message = new Message
            {
                LeftChatMember = new User { Id = 234234 } 
            }
        };
        public static Update updateCallbackQuery => new Update
        {
            CallbackQuery = new CallbackQuery
            {
                Data = "bbb"
            }
        };
        public static Update updateRandomCallbackQuery => new Update
        {
            CallbackQuery = new CallbackQuery
            {
                Data = "dsfsdf"
            }
        };
    }

    public class CallbackQueryFilterTest
    {
        [Fact]
        public void TestCallbackQueryWithSpecifiedData()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateCallbackQuery);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler2.updateCallbackQueryFirst));
        }

        [Fact]
        public void TestCallbackQueryType()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateRandomCallbackQuery);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler2.updateCallbackQuerySecond));
        }

        [Fact]
        public void TestCallbackQueryWithSpecifiedDataAll()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateCallbackQuery);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.updateCallbackQueryFirst));
        }

        [Fact]
        public void TestCallbackQueryTypeAll()
        {
            //ITelegramBotClient telegramBotClient;// = new TelegramBotClient();
            // Create mock update type

            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateRandomCallbackQuery);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.updateCallbackQuerySecond));
        }
    }

    public class MessageFilterTest
    {
        [Fact]
        public void TestStartMessagefilter()
        {
            //ITelegramBotClient telegramBotClient;// = new TelegramBotClient();
            var handlerDescriptors = MessageInvoker.CollectHandlers();
            
            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateStart);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.Start));
        }

        [Fact]
        public void TestTestMessagefilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateTest);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.test));
        }

        [Fact]
        public void TestChatMembersAddedMessagefilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateChatMembersAdded);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.updateChatMembersAdded));
        }

        [Fact]
        public void TestChatMemberLeftMessagefilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateChatMembersLeft);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.updateChatMemberLeft));
        }

        [Fact]
        public void TestAllUpdateCallbackQueryMessagefilter()
        {
            // Create mock update type
            var handlerDescriptors = MessageInvoker.CollectHandlers();

            FastMethodInfo matchedHandlerMethod = MessageInvoker.MatchHandlerMethod(handlerDescriptors.First().GetMethodInfos(), UpdateBuilder.updateCallbackQuery);

            Assert.True(matchedHandlerMethod.GetMethodInfo().Name == nameof(TestHandler.updateCallbackQueryFirst));
        }
    }
}
