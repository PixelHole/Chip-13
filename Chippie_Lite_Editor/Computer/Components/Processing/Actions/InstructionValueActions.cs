using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionValueActions
{
    internal static void Move(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var a = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        a.Content = b;
    }
    internal static void Add(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = a + b;
    }
    internal static void Subtract(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = a - b;
    }
    internal static void Multiply(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        long result = a * b;

        Register high = RegisterBank.GetRegister("High")!;
        Register low = RegisterBank.GetRegister("low")!;

        low.Content = (int)result;
        long shift = (long)Math.Pow(2, 32);
        result /= shift;
        high.Content = (int)result;
    }
    internal static void Power(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var a = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[1]]);

        long result = (int)Math.Pow(a, b);

        Register high = RegisterBank.GetRegister("High")!;
        Register low = RegisterBank.GetRegister("low")!;

        low.Content = (int)result;
        long shift = (long)Math.Pow(2, 32);
        result /= shift;
        high.Content = (int)result;
    }
    internal static void Divide(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = a / b;
    }
    internal static void Remainder(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = a % b;
    }
    internal static void ShiftLeftLogic(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);

        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        int value = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        int amount = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = BitMath.ShiftLeft(value, amount);
    }
    internal static void ShiftRightLogic(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);

        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        int value = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        int amount = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = BitMath.ShiftRight(value, amount);
    }
    internal static void ShiftLeftArithmatic(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);

        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        int value = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        int amount = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = BitMath.ShiftLeft(value, amount, true);
    }
    internal static void ShiftRightArithmatic(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 3);

        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        int value = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        int amount = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.Content = BitMath.ShiftRight(value, amount, true);
    }
}