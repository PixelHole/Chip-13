using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class ConditionEvaluationException : InstructionInterpretationException
{
    public new InstructionAction Action { get; private set; }

    public ConditionEvaluationException(InstructionAction action)
    {
        Action = action;
    }
    public ConditionEvaluationException(string? message, InstructionAction action) : base(message)
    {
        Action = action;
    }
    public ConditionEvaluationException(string? message, Exception? innerException, InstructionAction action) : base(message, innerException)
    {
        Action = action;
    }
}