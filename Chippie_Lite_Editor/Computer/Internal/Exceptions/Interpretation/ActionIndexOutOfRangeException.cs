using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class ActionIndexOutOfRangeException(int givenIndex, int argumentCount) : InstructionInterpretationException
{
    public int GivenIndex { get; private set; } = givenIndex;
    public int ArgumentCount { get; private set; } = argumentCount;
}