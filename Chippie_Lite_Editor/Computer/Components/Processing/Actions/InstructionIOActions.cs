using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionIOActions
{
    internal static void GetSerialInput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var reg = InstructionArgument.GetRegister(arguments[action.Indices[0]]);

        int input = SerialIO.GetInput();

        reg.Content = input;
    }
    internal static void SendSerialOutput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        SerialIO.BufferOutput([val]);
    }
}