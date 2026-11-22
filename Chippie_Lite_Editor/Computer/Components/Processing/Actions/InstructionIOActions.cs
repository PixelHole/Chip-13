using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Utility;
using wpf_Console;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionIOActions
{ 
    internal static void GetSerialInput(InstructionAction action)
    {
        long input = IOBuffer.GetInput().Result;

        ActionUtility.InterpretWrite(action.Arguments[0], input, action.Instruction);
    }
    internal static void GetKeyInput(InstructionAction action)
    {
        int input = (int)IOBuffer.GetKeyInput().Result;

        ActionUtility.InterpretWrite(action.Arguments[0], input, action.Instruction);
    }
    internal static void SendSerialOutput(InstructionAction action)
    {
        var val = ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);
        
        IOBuffer.BufferOutput([val]);
    }
    internal static void FlushIOBuffers(InstructionAction action)
    {
        IOBuffer.FullFlush();
    }
    internal static void SetCursorForeground(InstructionAction action)
    {
        var val = ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);

        IOInterface.SetForegroundIndex((int)val);
    }
    internal static void SetCursorBackground(InstructionAction action)
    {
        var val = ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);
        
        IOInterface.SetBackgroundIndex((int)val);
    }
    internal static void SetConsoleBackground(InstructionAction action)
    {
        var val = ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);
        
        IOInterface.SetConsoleBackgroundIndex((int)val);
    }
    internal static void SetCursor(InstructionAction action)
    {
        var x = (int)ActionUtility.InterpretRead(action.Arguments[0], action.Instruction);
        var y = (int)ActionUtility.InterpretRead(action.Arguments[1], action.Instruction);
        
        IOInterface.SetCursor(new Vector2Int(x, y));
    }
    
    internal static void ClearConsole(InstructionAction action)
    {
        IOInterface.ClearConsole();
        IOInterface.SetCursor(new Vector2Int(0, 0));
    }
}