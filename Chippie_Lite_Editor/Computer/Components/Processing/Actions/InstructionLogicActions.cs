using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionLogicActions
{
    internal static void And(InstructionAction action)
    {
        ActionUtility.CheckArgumentAndActionIndexCount(action, 3);
        
        var a = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], a & b, action.Instruction);
    }
    internal static void Or(InstructionAction action)
    {
        ActionUtility.CheckArgumentAndActionIndexCount(action, 3);
        
        var a = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], a | b, action.Instruction);
    }
    internal static void Xor(InstructionAction action)
    {
        ActionUtility.CheckArgumentAndActionIndexCount(action, 3);
        
        var a = ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        var b = ActionUtility.InterpretRead(action.Arguments[2], action.Instruction);
        
        ActionUtility.InterpretWrite(action.Arguments[0], a ^ b, action.Instruction);
    }
}