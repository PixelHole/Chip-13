using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class NotEnoughArgumentsException : InvalidInstructionException
{
    public int ExpectedCount { get; private set; }
    public int ProvidedCount { get; private set; }


    public NotEnoughArgumentsException(int actualLine, int expectedCount, int providedCount) : base(actualLine)
    {
        ExpectedCount = expectedCount;
        ProvidedCount = providedCount;
    }
    protected NotEnoughArgumentsException(SerializationInfo info, StreamingContext context, int actualLine, int expectedCount, int providedCount) : base(info, context, actualLine)
    {
        ExpectedCount = expectedCount;
        ProvidedCount = providedCount;
    }
    public NotEnoughArgumentsException(string? message, int actualLine, int expectedCount, int providedCount) : base(message, actualLine)
    {
        ExpectedCount = expectedCount;
        ProvidedCount = providedCount;
    }
    public NotEnoughArgumentsException(string? message, Exception? innerException, int actualLine, int expectedCount, int providedCount) : base(message, innerException, actualLine)
    {
        ExpectedCount = expectedCount;
        ProvidedCount = providedCount;
    }
}