using System.Text;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionActionConditions
{
    private static ConditionOperator[] Operators { get; } =
    [
        new("||", 1, 2, (inputs, _) => inputs[0] == "true" || inputs[1] == "true"),
        new("&&", 1, 2, (inputs, _) => inputs[0] == "true" && inputs[1] == "true"),
        new("!", 2, 1, (inputs, _) => inputs[0] == "true"),
        new("==", 2, 2, (inputs, ins) => 
                ActionUtility.InterpretRead(inputs[0], ins) == ActionUtility.InterpretRead(inputs[1], ins)),
        new("!=", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) != ActionUtility.InterpretRead(inputs[1], ins)),
        new("<", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) < ActionUtility.InterpretRead(inputs[1], ins)),
        new(">", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) > ActionUtility.InterpretRead(inputs[1], ins)),
        new("<=", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) <= ActionUtility.InterpretRead(inputs[1], ins)),
        new(">=", 2, 2, (inputs, ins) => 
            ActionUtility.InterpretRead(inputs[0], ins) > ActionUtility.InterpretRead(inputs[1], ins))
    ];
    
    public static bool EvaluateCondition(InstructionAction action)
    {
        if (string.IsNullOrEmpty(action.Condition) || string.IsNullOrWhiteSpace(action.Condition)) return true;

        var operands = ToRPN(action.Condition);

        Stack<ConditionContainer> wordStack = [];
        while (operands.Count != 0)
        {
            var operand = operands.Dequeue();
            if (operand.Operator == null)
            {
                wordStack.Push(operand);
                continue;
            }

            var op = operand.Operator;
            string[] args = new string[op.Value.InputCount];
            for (int i = 0; i < op.Value.InputCount; i++)
            {
                args[i] = wordStack.Pop().Number;
            }

            var result = op.Value.Operate(args, action.Instruction);
            wordStack.Push(new ConditionContainer(result, null));
        }

        while (wordStack.Count >= 2)
        {
            var a = wordStack.Pop();
            var b = wordStack.Pop();

            var result = ActionUtility.InterpretRead(a.Number, action.Instruction) ==
                         ActionUtility.InterpretRead(b.Number, action.Instruction) ? "true" : "false";
        }

        return wordStack.Peek().Number == "true";
    }
    public static Queue<ConditionContainer> ToRPN(string condition)
    {
        Queue<ConditionContainer> result = [];
        Stack<ConditionOperator> ops = [];
        var words = ExtractAllWords(condition);

        foreach (var word in words)
        {
            switch (word)
            {
                case "(":
                    ops.Push(new ConditionOperator("(", Byte.MaxValue, 0, (_, _) => false));
                    continue;
                case ")":
                {
                    while (ops.TryPeek(out var topOp) && topOp.Sign != "(")
                    {
                        result.Enqueue(new ConditionContainer("", ops.Pop()));
                    }

                    if (ops.Count > 0) ops.Pop();
                    continue;
                }
            }

            if (!IsOperator(word, out var op))
            {
                result.Enqueue(new ConditionContainer(word, null));
                continue;
            }

            while (ops.TryPeek(out var topOp) && topOp.Precedence <= op!.Value.Precedence)
            {
                result.Enqueue(new ConditionContainer("", topOp));
                ops.Pop();
            }
            ops.Push(op!.Value);
        }

        foreach (var op in ops)
        {
            result.Enqueue(new ConditionContainer("", op));
        }
        
        return result;
    }
    public static string[] ExtractAllWords(string condition)
    {
        List<string> words = [];
        StringBuilder buffer = new StringBuilder();
        
        foreach (var c in condition.Where(c => !char.IsWhiteSpace(c)))
        {
            if (c is '(' or ')')
            {
                if (buffer.Length > 0) words.Add(buffer.ToString());
                buffer.Clear();
                words.Add(c.ToString());
                continue;
            }
            
            if (IsOperator(buffer.ToString(), out _))
            {
                words.Add(buffer.ToString());
                buffer.Clear();
            }

            if (EndsWithOperator(buffer.ToString(), out var op) && !EndsWithOperator(buffer.ToString() + c, out _))
            {
                buffer.Remove(buffer.Length - op.Length, op.Length);
                words.Add(buffer.ToString());
                buffer.Clear();
                words.Add(op);
            }

            buffer.Append(c);
        }

        if (buffer.Length == 0) return words.ToArray();
        
        if (EndsWithOperator(buffer.ToString(), out var oper))
        {
            buffer.Remove(buffer.Length - oper.Length, oper.Length);
            words.Add(buffer.ToString());
            buffer.Clear();
            words.Add(oper);
        }
        else words.Add(buffer.ToString());
        
        return words.ToArray();
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
    private static bool EndsWithOperator(string inp, out string op)
    {
        foreach (var oper in Operators)
        {
            if (!inp.EndsWith(oper.Sign)) continue;
            op = oper.Sign;
            return true;
        }

        op = "";
        return false;
    }
}