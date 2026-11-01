using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components;

public struct ConditionOperator
{
    public string Sign { get; private set; }
    public byte Precedence { get; private set; }
    
    public byte InputCount { get; private init; }

    public delegate bool Action(string[] lists, Instruction instruction);
    public readonly Action ActionResult;


    public ConditionOperator(string sign, byte precedence, byte inputCount, Action actionResult)
    {
        Sign = sign;
        Precedence = precedence;
        InputCount = inputCount;
        ActionResult = actionResult;
    }

    public string Operate(string[] lists, Instruction instruction)
    {
        return ActionResult.Invoke(lists, instruction) ? "true" : "false";
    }
}