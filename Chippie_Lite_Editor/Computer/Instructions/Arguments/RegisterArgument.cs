using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.Computer.Instructions.Arguments;

public class RegisterArgument : InstructionArgument
{
    public Register? Register { get; private set; }


    public RegisterArgument(Register? register = null)
    {
        Register = register;
    }

    public override string ToString()
    {
        return Register == null ? "None" : Register.Name;
    }
}