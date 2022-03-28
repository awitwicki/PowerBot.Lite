using PowerBot.Lite.Attributes;
using PowerBot.Lite.HandlerInvokers;
using PowerBot.Lite.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Tests
{
    internal class TestHandler : BaseHandler
    {
        [MessageHandler("/start")]
        public async Task Start() { }

        [MessageHandler("/test")]
        public async Task test() { }

        [MessageTypeFilter(MessageType.ChatMembersAdded)]
        public async Task updateChatMembersAdded() { }

        [MessageTypeFilter(MessageType.ChatMemberLeft)]
        public async Task updateChatMemberLeft() { }

        [CallbackQueryHandler("bbb")]
        public async Task updateCallbackQuery() { }

        [UpdateTypeFilter(UpdateType.CallbackQuery)]
        public async Task updateTypeCallbackQuery() { }
    }
    internal class TestHandler2 : BaseHandler
    {
        [CallbackQueryHandler("bbb")]
        public async Task updateCallbackQuery() { }

        [UpdateTypeFilter(UpdateType.CallbackQuery)]
        public async Task updateTypeCallbackQuery() { }
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
    }
    
    public class MessageFilterTest
    {
        [Fact]
        public void TestStartMessagefilter()
        {
            //ITelegramBotClient telegramBotClient;// = new TelegramBotClient();

            Type commandHandlerType = typeof(TestHandler);
            List<MethodInfo> methodInfos = commandHandlerType.GetMethods().ToList();

            var filteredHandlerMethods = MessageInvoker.FilterHandlerMethods(methodInfos, UpdateBuilder.updateStart);

            Assert.True(filteredHandlerMethods.Count() == 1);
            Assert.True(filteredHandlerMethods.First().Name == methodInfos.First(x => x.Name == "Start").Name);
        }

        [Fact]
        public void TestTestMessagefilter()
        {
            ITelegramBotClient telegramBotClient;// = new TelegramBotClient();
            // create mock update type

            Type commandHandlerType = typeof(TestHandler);
            List<MethodInfo> methodInfos = commandHandlerType.GetMethods().ToList();

            var filteredHandlerMethods = MessageInvoker.FilterHandlerMethods(methodInfos, UpdateBuilder.updateTest);

            Assert.True(filteredHandlerMethods.Count() == 1);
            Assert.True(filteredHandlerMethods.First().Name == methodInfos.First(x => x.Name == "test").Name);
        }

        [Fact]
        public void TestChatMembersAddedMessagefilter()
        {
            ITelegramBotClient telegramBotClient;// = new TelegramBotClient();
            // create mock update type

            Type commandHandlerType = typeof(TestHandler);
            List<MethodInfo> methodInfos = commandHandlerType.GetMethods().ToList();

            var filteredHandlerMethods = MessageInvoker.FilterHandlerMethods(methodInfos, UpdateBuilder.updateChatMembersAdded);

            Assert.True(filteredHandlerMethods.Count() == 1);
            Assert.True(filteredHandlerMethods.First().Name == methodInfos.First(x => x.Name == "updateChatMembersAdded").Name);
        }

        [Fact]
        public void TestChatMemberLeftMessagefilter()
        {
            ITelegramBotClient telegramBotClient;// = new TelegramBotClient();
            // create mock update type

            Type commandHandlerType = typeof(TestHandler);
            List<MethodInfo> methodInfos = commandHandlerType.GetMethods().ToList();

            var filteredHandlerMethods = MessageInvoker.FilterHandlerMethods(methodInfos, UpdateBuilder.updateChatMembersLeft);

            Assert.True(filteredHandlerMethods.Count() == 1);
            Assert.True(filteredHandlerMethods.First().Name == methodInfos.First(x => x.Name == "updateChatMemberLeft").Name);
        }

        [Fact]
        public void TestAllUpdateCallbackQueryMessagefilter()
        {
            ITelegramBotClient telegramBotClient;// = new TelegramBotClient();
            // create mock update type

            Type commandHandlerType = typeof(TestHandler);
            List<MethodInfo> methodInfos = commandHandlerType.GetMethods().ToList();

            var filteredHandlerMethods = MessageInvoker.FilterHandlerMethods(methodInfos, UpdateBuilder.updateCallbackQuery);

            Assert.True(filteredHandlerMethods.Count() == 2);
            Assert.True(filteredHandlerMethods.All(x => x.Name.Contains("CallbackQuery")));            
        }
    }
}
