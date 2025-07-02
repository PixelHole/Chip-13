using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Instructions.Arguments;

public class NumberArgument : InstructionArgument
{
    public int Number { get; private set; }


    public NumberArgument(int number = 0)
    {
        Number = number;
    }

    public override string ToString()
    {
        return Number.ToString();
    }
}