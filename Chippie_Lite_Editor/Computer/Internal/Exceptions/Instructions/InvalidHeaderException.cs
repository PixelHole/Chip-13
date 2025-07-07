using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class InvalidHeaderException : InvalidInstructionException
{
    public string ProvidedHeader { get; private set; }


    public InvalidHeaderException(int actualLine, string providedHeader) : base(actualLine)
    {
        ProvidedHeader = providedHeader;
    }
    protected InvalidHeaderException(SerializationInfo info, StreamingContext context, int actualLine, string providedHeader) : base(info, context, actualLine)
    {
        ProvidedHeader = providedHeader;
    }
    public InvalidHeaderException(string? message, int actualLine, string providedHeader) : base(message, actualLine)
    {
        ProvidedHeader = providedHeader;
    }
    public InvalidHeaderException(string? message, Exception? innerException, int actualLine, string providedHeader) : base(message, innerException, actualLine)
    {
        ProvidedHeader = providedHeader;
    }
}