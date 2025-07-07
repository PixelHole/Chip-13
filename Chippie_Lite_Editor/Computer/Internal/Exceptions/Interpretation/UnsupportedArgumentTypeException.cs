using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class UnsupportedArgumentTypeException : InstructionInterpretationException
{
    public InstructionArgument Argument { get; private set; }


    public UnsupportedArgumentTypeException(InstructionArgument argument)
    {
        Argument = argument;
    }
    protected UnsupportedArgumentTypeException(SerializationInfo info, StreamingContext context, InstructionArgument argument) : base(info, context)
    {
        Argument = argument;
    }
    public UnsupportedArgumentTypeException(string? message, InstructionArgument argument) : base(message)
    {
        Argument = argument;
    }
    public UnsupportedArgumentTypeException(string? message, Exception? innerException, InstructionArgument argument) : base(message, innerException)
    {
        Argument = argument;
    }
}