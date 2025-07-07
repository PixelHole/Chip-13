using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Components.Actions;

public static class InstructionMiscActions
{
    public static void Beep(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 2);
        
        int freq = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        
        Console.Beep(freq, dur);
    }
    public static void Wait(InstructionAction action, IList<InstructionArgument> arguments)
    {
        InstructionActions.CheckArgumentAndActionIndexCount(action, arguments, 1);
        
        int dur = InstructionArgument.GetNumber(arguments[action.Indices[0]]);
        
        Thread.Sleep(dur);
    }
}