using System.Runtime.Serialization;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.File;

public class FileWriteException(Exception actualException) : Exception
{
    public Exception ActualException { get; private set; } = actualException;
}