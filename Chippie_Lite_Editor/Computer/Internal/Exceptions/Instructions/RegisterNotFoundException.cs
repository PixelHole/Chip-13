using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class RegisterNotFoundException(int actualLine, string providedId) : InvalidInstructionException(actualLine)
{
    public string ProvidedId { get; private set; } = providedId;
}