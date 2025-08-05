using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using wpf_Console;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionIOActions
{
    public delegate void BackgroundSetAction(int index);
    public static event BackgroundSetAction? SetBackgroundRequest;

    public delegate void ForegroundSetAction(int index);
    public static event ForegroundSetAction? SetForegroundRequest;

    public delegate void CursorSetAction(Vector2Int position);
    public static event CursorSetAction? SetCursorRequest;
    
    
    internal static void GetSerialInput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var reg = InstructionArgument.GetRegister(arguments[action.Indices[0]]);

        int input = SerialIO.GetInput().Result;

        reg.Content = input;
    }
    internal static void SendSerialOutput(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        SerialIO.BufferOutput([val]);
    }
    internal static void SetConsoleForeground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        SetForegroundRequest?.Invoke(val);
    }
    
    internal static void SetConsoleBackground(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        SetBackgroundRequest?.Invoke(val);
    }

    internal static void SetCursorLeft(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        SetCursorRequest?.Invoke(new Vector2Int(val, -1));
    }
    
    internal static void SetCursorTop(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        var val = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        SetCursorRequest?.Invoke(new Vector2Int(-1, val));
    }

    internal static void SetCursor(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        var x = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        var y = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        
        SetCursorRequest?.Invoke(new Vector2Int(x, y));
    }
}