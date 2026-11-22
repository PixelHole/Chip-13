using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components;

public class InternalAction
{
    public string Header { get; }
    public int ExpectedArgumentCount { get; }
    public Action<InstructionAction> Action { get; }
    public string[] ArgumentNames { get; }


    public InternalAction(string header, int expectedArgumentCount, Action<InstructionAction> action, string[] argumentNames)
    {
        Header = header;
        ExpectedArgumentCount = expectedArgumentCount;
        Action = action;
        ArgumentNames = argumentNames;
    }
}