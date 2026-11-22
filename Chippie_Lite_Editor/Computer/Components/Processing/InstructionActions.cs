using Chippie_Lite_WPF.Computer.Components.Actions;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionActions
{
    public static List<InternalAction> Actions { get; } =
    [
        new("add", 3, InstructionValueActions.Add, ["Destination", "operand 1", "operand 2"]),
        new("multiply", 4, InstructionValueActions.Multiply, ["High register", "Low register", "operand 1", "operand 2"]),
        new("power", 4, InstructionValueActions.Power, ["High register", "Low register", "operand 1", "operand 2"]),
        new("divide", 3, InstructionValueActions.Divide, ["Destination", "operand 1", "operand 2"]),
        new("remainder", 3, InstructionValueActions.Remainder, ["Destination", "operand 1", "operand 2"]),
        new("shift left logic", 3, InstructionValueActions.ShiftLeftLogic, ["Destination", "operand", "amount"]),
        new("shift right logic", 3, InstructionValueActions.ShiftRightLogic, ["Destination", "operand", "amount"]),
        new("shift left arithmatic", 3, InstructionValueActions.ShiftLeftArithmatic, ["Destination", "operand", "amount"]),
        new("shift right arithmatic", 3, InstructionValueActions.ShiftRightArithmatic, ["Destination", "operand", "amount"]),
        new("and", 3, InstructionLogicActions.And, ["Destination", "operand 1", "operand 2"]),
        new("or", 3, InstructionLogicActions.Or, ["Destination", "operand 1", "operand 2"]),
        new("xor", 3, InstructionLogicActions.Xor, ["Destination", "operand 1", "operand 2"]),
        new("serial input", 1, InstructionIOActions.GetSerialInput, ["Destination"]),
        new("key input", 1, InstructionIOActions.GetKeyInput, ["Destination"]),
        new("serial output", 1, InstructionIOActions.SendSerialOutput, ["Content"]),
        new("flush io buffers", 0, InstructionIOActions.FlushIOBuffers, []),
        new("cursor background", 1, InstructionIOActions.SetCursorBackground, ["Color"]),
        new("cursor foreground", 1, InstructionIOActions.SetCursorForeground, ["Color"]),
        new("console background", 1, InstructionIOActions.SetConsoleBackground, ["Color"]),
        new("console cursor", 2, InstructionIOActions.SetCursor, ["X", "Y"]),
        new("console clear", 0, InstructionIOActions.ClearConsole, []),
        new("wait", 1, InstructionMiscActions.Wait, ["Duration"]),
        new("beep", 2, InstructionMiscActions.Beep, ["Frequency", "Duration"]),
        new("audio signal", 3, InstructionMiscActions.AudioWave, ["Frequency", "Duration", "Waveform"]),
        new("terminate", 0, _ => Terminate(), []),
    ];
    
    public static void ExecuteAction(InstructionAction action)
    {
        var internalAction = FindAction(action.Header.ToLower());
        if (internalAction == null) throw new ActionNotFoundException(action.Header);
        ActionUtility.CheckArgumentAndActionIndexCount(action, internalAction.ExpectedArgumentCount);
        internalAction.Action.Invoke(action);
    }
    
    public static int IndexOf(string header) =>  Actions.FindIndex(action => string.Equals(action.Header, header, StringComparison.CurrentCultureIgnoreCase));
    public static InternalAction? FindAction(string header)
    {
        return Actions.FirstOrDefault(action => string.Equals(action.Header, header, StringComparison.CurrentCultureIgnoreCase));
    }
    
    private static void Terminate()
    {
        Chippie.HaltOperation();
    }
}