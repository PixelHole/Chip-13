using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.Computer.Instructions.Arguments;

public class RegisterNumberArgument : InstructionArgument
{
    public RegisterArgument RegisterArgument { get; private set; }
    public NumberArgument NumberArgument { get; private set; }


    public RegisterNumberArgument(Register? register = null, long num = 0)
    {
        RegisterArgument = new RegisterArgument(register);
        NumberArgument = new NumberArgument(num);
    }


    public long GetValue()
    {
        if (RegisterArgument.Register == null) return NumberArgument.Number;
        return NumberArgument.Number + RegisterArgument.Register.Content;
    }
    public Register GetRegister()
    {
        int index = (int)NumberArgument.Number;
        if (RegisterArgument.Register != null) index += RegisterBank.IndexOf(RegisterArgument.Register);
        return RegisterBank.GetRegister(index);
    }
    
    public override string ToString()
    {
        if (RegisterArgument.Register == null) return NumberArgument.Number.ToString();
        return $"{RegisterArgument.Register.Name} + {NumberArgument.Number}";
    }
}