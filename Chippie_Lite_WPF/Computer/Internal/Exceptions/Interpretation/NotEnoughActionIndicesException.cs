using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class NotEnoughActionIndicesException : InstructionInterpretationException
{
    public int ExpectedCount { get; private set; }
    public int GivenCount { get; private set; }


    public NotEnoughActionIndicesException(int expectedCount, int givenCount)
    {
        ExpectedCount = expectedCount;
        GivenCount = givenCount;
    }
    protected NotEnoughActionIndicesException(SerializationInfo info, StreamingContext context, int expectedCount, int givenCount) : base(info, context)
    {
        ExpectedCount = expectedCount;
        GivenCount = givenCount;
    }
    public NotEnoughActionIndicesException(string? message, int expectedCount, int givenCount) : base(message)
    {
        ExpectedCount = expectedCount;
        GivenCount = givenCount;
    }
    public NotEnoughActionIndicesException(string? message, Exception? innerException, int expectedCount, int givenCount) : base(message, innerException)
    {
        ExpectedCount = expectedCount;
        GivenCount = givenCount;
    }
}