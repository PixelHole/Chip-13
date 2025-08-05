using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class NotEnoughActionIndicesException(int expectedCount, int givenCount) : InstructionInterpretationException
{
    public int ExpectedCount { get; private set; } = expectedCount;
    public int GivenCount { get; private set; } = givenCount;
}