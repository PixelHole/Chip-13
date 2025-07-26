using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionMemoryActions
{
    internal static void Read(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var address = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        dest.Content = Memory.Read(address);
    }
    internal static void Write(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var address = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var content = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        Memory.Write(address, content);
    }
}