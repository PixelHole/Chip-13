using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class InvalidArgumentException(int actualLine, string inputString, InstructionArgumentType expectedType)
    : InvalidInstructionException(actualLine)
{
    public string InputString { get; private set; } = inputString;
    public InstructionArgumentType ExpectedType { get; private set; } = expectedType;
}