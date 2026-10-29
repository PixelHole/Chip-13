using System.Text;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionActionConditions
{
    private static ConditionOperator[] Operators { get; } = new[]
    {
        new ConditionOperator("(", 0, 0, (lists, instruction) => ""),
        new ConditionOperator(")", 0, 0, (lists, instruction) => ""),
        new ConditionOperator("||", 1, 2, (inputs, ins) => inputs[0] == "true" || inputs[1] == "true" ? "true" : "false"),
        new ConditionOperator("&&", 1, 2, (inputs, ins) => inputs[0] == "true" && inputs[1] == "true" ? "true" : "false"),
        new ConditionOperator("!", 2, 1, (inputs, ins) => inputs[0] == "true" ? "false" : "true"),
        new ConditionOperator("==", 2, 2, (inputs, ins) => 
                ActionUtility.InterpretRead(inputs[0], ins) == ActionUtility.InterpretRead(inputs[1], ins) ? "true" : "false"),
        new ConditionOperator("!=", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) != ActionUtility.InterpretRead(inputs[1], ins) ? "true" : "false"),
        new ConditionOperator("<", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) < ActionUtility.InterpretRead(inputs[1], ins) ? "true" : "false"),
        new ConditionOperator(">", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) > ActionUtility.InterpretRead(inputs[1], ins) ? "true" : "false"),
        new ConditionOperator("<=", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) <= ActionUtility.InterpretRead(inputs[1], ins) ? "true" : "false"),
        new ConditionOperator(">=", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) > ActionUtility.InterpretRead(inputs[1], ins) ? "true" : "false"),
    };
    
    public static bool EvaluateCondition(InstructionAction action)
    {
        if (string.IsNullOrEmpty(action.Condition) || string.IsNullOrWhiteSpace(action.Condition)) return true;

        var operands = ToRPN(action.Condition);
    }
    private static ConditionContainer[] ToRPN(string condition)
    {
        List<ConditionContainer> result = [];
        Stack<ConditionOperator> ops = [];
        var words = condition.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            if (!IsOperator(word, out var op))
            {
                result.Add(new ConditionContainer(word, null));
                continue;
            }
            if (ops.TryPop(out var topOp) && topOp.Precedence == op.Value.Precedence)
        }
    }
    private static bool IsOperator(string input, out ConditionOperator? op)
    {
        foreach (var oper in Operators)
        {
            if (oper.Sign != input) continue;
            op = oper;
            return true;
        }

        op = null;
        return false;
    }
}