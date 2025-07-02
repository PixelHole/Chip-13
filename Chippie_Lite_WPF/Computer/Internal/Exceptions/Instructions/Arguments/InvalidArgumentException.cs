using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class InvalidArgumentException : InvalidInstructionException
{
    public string InputString { get; private set; }
    public InstructionArgumentType ExpectedType { get; private set; }


    public InvalidArgumentException(int actualLine, string inputString, InstructionArgumentType expectedType) : base(actualLine)
    {
        InputString = inputString;
        ExpectedType = expectedType;
    }
    protected InvalidArgumentException(SerializationInfo info, StreamingContext context, int actualLine, string inputString, InstructionArgumentType expectedType) : base(info, context, actualLine)
    {
        InputString = inputString;
        ExpectedType = expectedType;
    }
    public InvalidArgumentException(string? message, int actualLine, string inputString, InstructionArgumentType expectedType) : base(message, actualLine)
    {
        InputString = inputString;
        ExpectedType = expectedType;
    }
    public InvalidArgumentException(string? message, Exception? innerException, int actualLine, string inputString, InstructionArgumentType expectedType) : base(message, innerException, actualLine)
    {
        InputString = inputString;
        ExpectedType = expectedType;
    }
}