using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using wpf_Console;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionIOActions
{ 
    internal static void GetSerialInput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var reg = InstructionArgument.GetRegister(arguments[action.Indices[0]]);

        long input = Chippie.MainIOBuffer.GetInput().Result;

        reg.Content = input;
    }
    internal static void GetKeyInput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var reg = InstructionArgument.GetRegister(arguments[action.Indices[0]]);

        int input = (int)Chippie.MainIOBuffer.GetKeyInput().Result;

        reg.Content = input;
    }
    internal static void SendSerialOutput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = (int)InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        Chippie.MainIOBuffer.BufferOutput([val]);
    }
    internal static void FlushIOBuffers(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 0);
        
        Chippie.MainIOBuffer.FullFlush();
    }
    internal static void SetCursorForeground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = (int)InstructionArgument.GetNumber(arguments[action.Indices[0]]);

        Chippie.MainIoInterface.SetForegroundIndex(val);
    }
    internal static void SetCursorBackground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = (int)InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        Chippie.MainIoInterface.SetBackgroundIndex(val);
    }
    internal static void SetConsoleBackground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = (int)InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        Chippie.MainIoInterface.SetConsoleBackgroundIndex(val);
    }
    internal static void SetCursorLeft(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = (int)InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        Chippie.MainIoInterface.SetCursorLeft(val);
    }
    internal static void SetCursorTop(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = (int)InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        Chippie.MainIoInterface.SetCursorTop(val);
    }
    internal static void SetCursor(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var x = (int)InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var y = (int)InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        
        Chippie.MainIoInterface.SetCursor(new Vector2Int(x, y));
    }
    
    internal static void ClearConsole(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 0);
        
        Chippie.MainIoInterface.ClearConsole();
        Chippie.MainIoInterface.SetCursor(new Vector2Int(0, 0));
    }
}