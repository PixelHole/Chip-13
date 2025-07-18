using Chippie_Lite_WPF.Computer.Components.Actions;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionActions
{
    public static void ExecuteAction(InstructionAction action, IList<InstructionArgument> arguments)
    {
        switch (action.Header.ToLower())
        {
            case "move" :
                InstructionValueActions.Move(action, arguments);
                break;
            
            case "add" :
                InstructionValueActions.Add(action, arguments);
                break;
            
            case "subtract" :
                InstructionValueActions.Subtract(action, arguments);
                break;
            
            case "multiply" :
                InstructionValueActions.Multiply(action, arguments);
                break;
            
            case "divide" :
                InstructionValueActions.Divide(action, arguments);
                break;
            
            case "remainder" :
                InstructionValueActions.Remainder(action, arguments);
                break;

            case "shift left logic" :
                InstructionValueActions.ShiftLeftLogic(action, arguments);
                break;

            case "shift right logic" :
                InstructionValueActions.ShiftRightLogic(action, arguments);
                break;
            
            case "shift left arithmatic" :
                InstructionValueActions.ShiftLeftArithmatic(action, arguments);
                break;

            case "shift right arithmatic" :
                InstructionValueActions.ShiftRightArithmatic(action, arguments);
                break;
            
            case "not" :
                InstructionLogicActions.Not(action, arguments);
                break;
            
            case "and" :
                InstructionLogicActions.And(action, arguments);
                break;
            
            case "nand" :
                InstructionLogicActions.Nand(action, arguments);
                break;
            
            case "or" :
                InstructionLogicActions.Or(action, arguments);
                break;
            
            case "xor" :
                InstructionLogicActions.Xor(action, arguments);
                break;
            
            case "nor" :
                InstructionLogicActions.Nor(action, arguments);
                break;
            
            case "nxor" :
                InstructionLogicActions.Nxor(action, arguments);
                break;
            
            case "jump" :
                InstructionJumpActions.Jump(action, arguments);
                break;
            
            case "jump relative" :
                InstructionJumpActions.JumpRelative(action, arguments);
                break;
            
            case "jump if equal" :
                InstructionJumpActions.JumpIfEqual(action, arguments);
                break;
            
            case "jump if equal relative" :
                InstructionJumpActions.JumpIfEqualRelative(action, arguments);
                break;
            
            case "jump if less" :
                InstructionJumpActions.JumpIfLess(action, arguments);
                break;
            
            case "jump if less relative" :
                InstructionJumpActions.JumpIfLessRelative(action, arguments);
                break;
            
            case "jump if greater" :
                InstructionJumpActions.JumpIfGreater(action, arguments);
                break;
            
            case "jump if greater relative" :
                InstructionJumpActions.JumpIfGreaterRelative(action, arguments);
                break;
            
            case "serial input" :
                InstructionIOActions.GetSerialInput(action, arguments);
                break;
            
            case "serial output" :
                InstructionIOActions.SendSerialOutput(action, arguments);
                break;
            
            case "wait" :
                InstructionMiscActions.Wait(action, arguments);
                break;
            
            case "beep" :
                InstructionMiscActions.Beep(action, arguments);
                break;
        }
    }
    

    
    internal static void CheckArgumentAndActionIndexCount(InstructionAction action, IList<InstructionArgument> arguments,
        int expected)
    {
        CheckActionIndexCount(action, expected);
        CheckArgumentCount(arguments, expected);
    }
    private static void CheckActionIndexCount(InstructionAction action, int expected)
    {
        if (action.Indices.Length < expected) throw new NotEnoughActionIndicesException(action.Indices.Length, expected);
    }
    private static void CheckArgumentCount(IList<InstructionArgument> arguments, int expected)
    {
        if (arguments.Count < expected) throw new NotEnoughActionIndicesException(arguments.Count, expected);
    }
}