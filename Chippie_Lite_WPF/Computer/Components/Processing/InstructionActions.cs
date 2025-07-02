using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionActions
{
    public static void ExecuteAction(InstructionAction action, IList<InstructionArgument> arguments)
    {
        switch (action.Header.ToLower())
        {
            case "add" :
                Add(action, arguments);
                break;
        }
    }

    private static void Add(InstructionAction action, IList<InstructionArgument> arguments)
    {
        CheckArgumentAndActionIndexCount(action, arguments, 3);
        
        var dest = InstructionArgument.GetRegister(arguments[action.Indices[0]]);
        var a = InstructionArgument.GetNumber(arguments[action.Indices[1]]);
        var b = InstructionArgument.GetNumber(arguments[action.Indices[2]]);
        
        dest.SetContent(a);
        dest.AddToContent(b);
    }

    private static void CheckArgumentAndActionIndexCount(InstructionAction action, IList<InstructionArgument> arguments,
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