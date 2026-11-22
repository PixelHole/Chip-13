using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

public class ActionNotFoundException : Exception
{
    public string ActionHeader { get; set; }
    public ActionNotFoundException(string actionHeader)
    {
        ActionHeader = actionHeader;
    }
    protected ActionNotFoundException(SerializationInfo info, StreamingContext context, string actionHeader) : base(info, context)
    {
        ActionHeader = actionHeader;
    }
    public ActionNotFoundException(string? message, string actionHeader) : base(message)
    {
        ActionHeader = actionHeader;
    }
    public ActionNotFoundException(string? message, Exception? innerException, string actionHeader) : base(message, innerException)
    {
        ActionHeader = actionHeader;
    }
}