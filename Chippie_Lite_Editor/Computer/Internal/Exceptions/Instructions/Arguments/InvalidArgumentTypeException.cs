using System.Runtime.Serialization;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal.Exceptions;

public class InvalidArgumentTypeException(int actualLine, InstructionArgumentType providedType)
    : InvalidInstructionException(actualLine)
{
    public InstructionArgumentType ProvidedType { get; private set; } = providedType;
}