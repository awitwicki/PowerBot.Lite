using System;
using System.Reflection;
using Telegram.Bot.Types;
using Xunit;

namespace Tests.InitialData;

public class UpdateWithComparedHandlerMethodList : TheoryData<Update, Type, MethodInfo>
{
    public UpdateWithComparedHandlerMethodList()
    {
        // TestHandler2
        Add(UpdateBuilder.UpdateCallbackQueryLorem,
            typeof(TestHandler2),
            typeof(TestHandler2).GetMethod(nameof(TestHandler2.UpdateCallbackQueryLorem)));
        
        Add(UpdateBuilder.UpdateRandomCallbackQueryLoremIpsum,
            typeof(TestHandler2),
            typeof(TestHandler2).GetMethod(nameof(TestHandler2.UpdateCallbackQueryLoremIpsum)));
        
        Add(UpdateBuilder.UpdateRandomCallbackQueryGlobal,
            typeof(TestHandler2),
            typeof(TestHandler2).GetMethod(nameof(TestHandler2.UpdateCallbackQueryGlobal)));
        
        // TestHandler
        Add(UpdateBuilder.UpdateStart,
            typeof(TestHandler),
            typeof(TestHandler).GetMethod(nameof(TestHandler.Start)));
        
        Add(UpdateBuilder.UpdateTest,
            typeof(TestHandler),
            typeof(TestHandler).GetMethod(nameof(TestHandler.Test)));
        
        Add(UpdateBuilder.UpdateCallbackQueryBbb,
            typeof(TestHandler),
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateCallbackQueryDataBbb)));
        
        Add(UpdateBuilder.UpdateRandomCallbackQueryGlobal,
            typeof(TestHandler),
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateCallbackQueryGlobal)));
        
        Add(UpdateBuilder.UpdateChatMembersAdded,
            typeof(TestHandler),
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateChatMembersAdded)));
        
        Add(UpdateBuilder.UpdateChatMembersLeft,
            typeof(TestHandler),
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateChatMemberLeft)));
    }
}
