using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionJumpActions
{
    internal static void Jump(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);

        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip!.Content = val - 2;
    }
    internal static void JumpRelative(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);

        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip!.Content += val - 2;
    }
    internal static void JumpIfEqualRelative(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var jump = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        if (a != b) return;
        
        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip.Content += jump - 2;
    }
    internal static void JumpIfEqual(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var jump = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        if (a != b) return;
        
        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip.Content = jump - 2;
    }
    internal static void JumpIfLessRelative(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var jump = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        if (a >= b) return;
        
        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip.Content += jump - 2;
    }
    internal static void JumpIfLess(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var jump = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        if (a >= b) return;
        
        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip.Content = jump - 2;
    }
    internal static void JumpIfGreaterRelative(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var jump = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        if (a <= b) return;
        
        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip.Content += jump - 2;
    }
    internal static void JumpIfGreater(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var jump = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        if (a <= b) return;
        
        var ip = RegisterBank.GetRegister("Instruction Pointer");

        ip.Content = jump - 2;
    }
}