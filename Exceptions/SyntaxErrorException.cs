using System;
using System.Runtime.Serialization;

namespace Calculator.Exceptions;

public class SyntaxErrorException : Exception
{
    public SyntaxErrorException()
    {
    }
    protected SyntaxErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    public SyntaxErrorException(string? message) : base(message)
    {
    }
    public SyntaxErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}