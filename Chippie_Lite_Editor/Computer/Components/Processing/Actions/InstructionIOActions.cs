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

        int input = IOBuffer.GetInput().Result;

        reg.Content = input;
    }
    internal static void GetKeyInput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var reg = InstructionArgument.GetRegister(arguments[action.Indices[0]]);

        int input = (int)IOBuffer.GetKeyInput().Result;

        reg.Content = input;
    }
    internal static void SendSerialOutput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        IOBuffer.BufferOutput([val]);
    }
    internal static void FlushIOBuffers(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 0);
        
        IOBuffer.FullFlush();
    }
    internal static void SetCursorForeground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);

        IOInterface.SetForegroundIndex(val);
    }
    internal static void SetCursorBackground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        IOInterface.SetBackgroundIndex(val);
    }
    internal static void SetConsoleBackground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        IOInterface.SetConsoleBackgroundIndex(val);
    }
    internal static void SetCursorLeft(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        IOInterface.SetCursorLeft(val);
    }
    internal static void SetCursorTop(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        IOInterface.SetCursorTop(val);
    }
    internal static void SetCursor(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var x = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var y = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        
        IOInterface.SetCursor(new Vector2Int(x, y));
    }
}