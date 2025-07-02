using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class RegisterNotFoundException : InvalidInstructionException
{
    public string ProvidedId { get; private set; }


    public RegisterNotFoundException(int actualLine, string providedId) : base(actualLine)
    {
        ProvidedId = providedId;
    }
    protected RegisterNotFoundException(SerializationInfo info, StreamingContext context, int actualLine, string providedId) : base(info, context, actualLine)
    {
        ProvidedId = providedId;
    }
    public RegisterNotFoundException(string? message, int actualLine, string providedId) : base(message, actualLine)
    {
        ProvidedId = providedId;
    }
    public RegisterNotFoundException(string? message, Exception? innerException, int actualLine, string providedId) : base(message, innerException, actualLine)
    {
        ProvidedId = providedId;
    }
}