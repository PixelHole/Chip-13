using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class InvalidArgumentTypeException : InvalidInstructionException
{
    public InstructionArgumentType ProvidedType { get; private set; }


    public InvalidArgumentTypeException(int actualLine, InstructionArgumentType providedType) : base(actualLine)
    {
        ProvidedType = providedType;
    }
    protected InvalidArgumentTypeException(SerializationInfo info, StreamingContext context, int actualLine, InstructionArgumentType providedType) : base(info, context, actualLine)
    {
        ProvidedType = providedType;
    }
    public InvalidArgumentTypeException(string? message, int actualLine, InstructionArgumentType providedType) : base(message, actualLine)
    {
        ProvidedType = providedType;
    }
    public InvalidArgumentTypeException(string? message, Exception? innerException, int actualLine, InstructionArgumentType providedType) : base(message, innerException, actualLine)
    {
        ProvidedType = providedType;
    }
}