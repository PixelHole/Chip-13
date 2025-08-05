using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class UnsupportedArgumentTypeException(InstructionArgument argument) : InstructionInterpretationException
{
    public InstructionArgument Argument { get; private set; } = argument;
}