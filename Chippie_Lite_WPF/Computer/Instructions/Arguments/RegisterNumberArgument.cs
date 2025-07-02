using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Instructions.Arguments;

public class RegisterNumberArgument : InstructionArgument
{
    public RegisterArgument RegisterArgument { get; private set; }
    public NumberArgument NumberArgument { get; private set; }


    public RegisterNumberArgument(Register? register = null, int num = 0)
    {
        RegisterArgument = new RegisterArgument(register);
        NumberArgument = new NumberArgument(num);
    }


    public int GetValue()
    {
        if (RegisterArgument.Register == null) return NumberArgument.Number;
        return NumberArgument.Number + RegisterArgument.Register.Content;
    }
    public Register? GetRegister()
    {
        int index = NumberArgument.Number;
        if (RegisterArgument.Register != null) index += RegisterBank.IndexOf(RegisterArgument.Register);
        return RegisterBank.GetRegister(index);
    }
    
    public override string ToString()
    {
        return $"{RegisterArgument.ToString()} : {NumberArgument.ToString()}";
    }
}