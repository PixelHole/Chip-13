namespace Chippie_Lite_WPF.Computer.Instructions.Arguments;

public class NumberArgument : InstructionArgument
{
    public long Number { get; private set; }


    public NumberArgument(long number = 0)
    {
        Number = number;
    }

    public override string ToString()
    {
        return Number.ToString();
    }
}