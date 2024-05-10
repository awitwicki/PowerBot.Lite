using System;
using System.Collections.Generic;
using System.Reflection;
using Telegram.Bot.Types;
using Xunit;

namespace PowerBot.Lite.Tests.InitialData;

public class UpdateWithComparedHandlersMethodList : TheoryData<Update, List<Type>, MethodInfo>
{
    public UpdateWithComparedHandlersMethodList()
    {
        // TestHandler2
        Add(UpdateBuilder.UpdateCallbackQueryLorem,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateCallbackQueryGlobal)));
        
        Add(UpdateBuilder.UpdateRandomCallbackQueryLoremIpsum,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateCallbackQueryGlobal)));
        
        Add(UpdateBuilder.UpdateRandomCallbackQueryGlobal,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateCallbackQueryGlobal)));
        
        // TestHandler
        Add(UpdateBuilder.UpdateStart,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.Start)));
        
        Add(UpdateBuilder.UpdateTest,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.Test)));
        
        Add(UpdateBuilder.UpdateCallbackQueryBbb,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateCallbackQueryDataBbb)));
        
        Add(UpdateBuilder.UpdateRandomCallbackQueryGlobal,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateCallbackQueryGlobal)));
        
        Add(UpdateBuilder.UpdateChatMembersAdded,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateChatMembersAdded)));
        
        Add(UpdateBuilder.UpdateChatMembersLeft,
            new List<Type> { typeof(TestHandler), typeof(TestHandler2)},
            typeof(TestHandler).GetMethod(nameof(TestHandler.UpdateChatMemberLeft)));
    }
}
