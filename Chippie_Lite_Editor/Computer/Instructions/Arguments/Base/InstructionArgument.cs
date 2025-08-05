using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

namespace Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

public abstract class InstructionArgument
{
    public static Register GetRegister(InstructionArgument argument)
    {
        switch (argument)
        {
            case RegisterArgument registerArgument:
                return registerArgument.Register!;
            case NumberArgument numberArgument:
                return RegisterBank.GetRegister(numberArgument.Number);
            case RegisterNumberArgument registerNumberArgument:
                return registerNumberArgument.GetRegister();
            default:
                throw new UnsupportedArgumentTypeException(argument);
        }
    }
    public static int GetNumber(InstructionArgument argument)
    {
        switch (argument)
        {
            case RegisterArgument registerArgument:
                return registerArgument.Register!.Content;
            case NumberArgument numberArgument:
                return numberArgument.Number;
            case RegisterNumberArgument registerNumberArgument:
                return registerNumberArgument.GetValue();
            default:
                throw new UnsupportedArgumentTypeException(argument);
        }
    }
}