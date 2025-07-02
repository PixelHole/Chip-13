using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

public class InvalidInstructionException : Exception
{
    public int ActualLine { get; protected set; }


    public InvalidInstructionException(int actualLine)
    {
        ActualLine = actualLine;
    }
    protected InvalidInstructionException(SerializationInfo info, StreamingContext context, int actualLine) : base(info, context)
    {
        ActualLine = actualLine;
    }
    public InvalidInstructionException(string? message, int actualLine) : base(message)
    {
        ActualLine = actualLine;
    }
    public InvalidInstructionException(string? message, Exception? innerException, int actualLine) : base(message, innerException)
    {
        ActualLine = actualLine;
    }


    public void SetLine(int line) => ActualLine = line;
}