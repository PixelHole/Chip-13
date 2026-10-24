using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionMemoryActions
{
    internal static void Read(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var address = (int)InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        int moduleIndex = action.Indices[2];

        var modules = ChippieComponentManager.GetComponentsOfType<Memory>();
        
        dest.Content = modules[moduleIndex].Read(address);
    }
    internal static void Write(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var content = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var address = (int)InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        int moduleIndex = action.Indices[2];

        var modules = ChippieComponentManager.GetComponentsOfType<Memory>();
        
        modules[moduleIndex].Write(address, content);
    }
}