using System;

namespace PowerBot.Lite.Exceptions;

public class NotDefinedAnyHandlersOrMiddlewaresException : Exception
{
    public NotDefinedAnyHandlersOrMiddlewaresException()
    {
    }

    public NotDefinedAnyHandlersOrMiddlewaresException(string message)
        : base(message)
    {
    }

    public NotDefinedAnyHandlersOrMiddlewaresException(string message, Exception inner)
        : base(message, inner)
    {
    }    
}
