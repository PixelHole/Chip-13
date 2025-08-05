using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class InstructionInterpretationException : Exception
{
    public Instruction Instruction { get; set; }
    public InstructionAction Action { get; set; }
    public int ActionIndex { get; set; }


    public InstructionInterpretationException()
    {
    }
    [Obsolete("Obsolete")]
    protected InstructionInterpretationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    public InstructionInterpretationException(string? message) : base(message)
    {
    }
    public InstructionInterpretationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}