using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Instruction_Sets;

public class TooManyInstructionSetsException : Exception
{
    public TooManyInstructionSetsException()
    {
    }
    protected TooManyInstructionSetsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    public TooManyInstructionSetsException(string? message) : base(message)
    {
    }
    public TooManyInstructionSetsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}