using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionLogicActions
{
    internal static void Not(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        dest.Content = ~a;
    }
    internal static void And(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = a & b;
    }
    internal static void Nand(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = ~(a & b);
    }
    internal static void Or(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);

        dest.Content = a | b;
    }
    internal static void Xor(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = a ^ b;
    }
    internal static void Nxor(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = ~(a ^ b);
    }
    internal static void Nor(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = ~(a | b);
    }
}