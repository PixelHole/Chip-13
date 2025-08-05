using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class InvalidHeaderException(int actualLine, string providedHeader) : InvalidInstructionException(actualLine)
{
    public string ProvidedHeader { get; private set; } = providedHeader;
}