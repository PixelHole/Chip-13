namespace Chippie_Lite_WPF.Computer.Internal.Exceptions.File;

public class FileOpenException(Exception actualException) : Exception
{
    public Exception ActualException { get; private set; } = actualException;
}