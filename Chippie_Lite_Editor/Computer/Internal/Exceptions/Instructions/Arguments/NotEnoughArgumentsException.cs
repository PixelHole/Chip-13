using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class NotEnoughArgumentsException(int actualLine, int expectedCount, int providedCount)
    : InvalidInstructionException(actualLine)
{
    public int ExpectedCount { get; private set; } = expectedCount;
    public int ProvidedCount { get; private set; } = providedCount;
}