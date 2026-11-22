using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class InternalActionNotFoundException : Exception
{
    public string Header { get; }

    public InternalActionNotFoundException(string header)
    {
        Header = header;
    }
    protected InternalActionNotFoundException(SerializationInfo info, StreamingContext context, string header) : base(info, context)
    {
        Header = header;
    }
    public InternalActionNotFoundException(string? message, string header) : base(message)
    {
        Header = header;
    }
    public InternalActionNotFoundException(string? message, Exception? innerException, string header) : base(message, innerException)
    {
        Header = header;
    }
}