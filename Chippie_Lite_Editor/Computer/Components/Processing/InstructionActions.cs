using Chippie_Lite_WPF.Computer.Components.Actions;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionActions
{
    public static void ExecuteAction(InstructionAction action)
    {
        switch (action.Header.ToLower())
        {
            case "add" :
                InstructionValueActions.Add(action);
                break;
            
            case "multiply" :
                InstructionValueActions.Multiply(action);
                break;
            
            case "power" :
                InstructionValueActions.Power(action);
                break;
            
            case "divide" :
                InstructionValueActions.Divide(action);
                break;
            
            case "remainder" :
                InstructionValueActions.Remainder(action);
                break;

            case "shift left logic" :
                InstructionValueActions.ShiftLeftLogic(action);
                break;

            case "shift right logic" :
                InstructionValueActions.ShiftRightLogic(action);
                break;
            
            case "shift left arithmatic" :
                InstructionValueActions.ShiftLeftArithmatic(action);
                break;

            case "shift right arithmatic" :
                InstructionValueActions.ShiftRightArithmatic(action);
                break;
            
            case "and" :
                InstructionLogicActions.And(action);
                break;
            
            case "or" :
                InstructionLogicActions.Or(action);
                break;
            
            case "xor" :
                InstructionLogicActions.Xor(action);
                break;
            
            case "serial input" :
                InstructionIOActions.GetSerialInput(action);
                break;
            
            case "key input" :
                InstructionIOActions.GetKeyInput(action);
                break;
            
            case "serial output" :
                InstructionIOActions.SendSerialOutput(action);
                break;
            
            case "flush io buffers" :
                InstructionIOActions.FlushIOBuffers(action);
                break;
            
            case "cursor background" :
                InstructionIOActions.SetCursorBackground(action);
                break;
            
            case "cursor foreground" :
                InstructionIOActions.SetCursorForeground(action);
                break;
            
            case "console background" :
                InstructionIOActions.SetConsoleBackground(action);
                break;
            
            case "console cursor" :
                InstructionIOActions.SetCursor(action);
                break;
            
            case "console clear" :
                InstructionIOActions.ClearConsole(action);
                break;
            
            case "wait" :
                InstructionMiscActions.Wait(action);
                break;
            
            case "beep" :
                InstructionMiscActions.Beep(action);
                break;
            
            case "audio signal" :
                InstructionMiscActions.AudioWave(action);
                break;
            
            case "terminate" :
                Terminate();
                return;
        }
    }
    
    private static void Terminate()
    {
        Chippie.HaltOperation();
    }
}