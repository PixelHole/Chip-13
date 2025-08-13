using System.Text;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Instructions;

public struct Instruction : IEquatable<Instruction>
{
    public string Header { get; private set; }
    public List<InstructionArgument> Arguments { get; private set; } = new List<InstructionArgument>();
    public List<InstructionAction> Actions { get; private set; } = new List<InstructionAction>();
    public int ScriptLine { get; set; }
    public bool HasBreakpoint { get; set; }


    public Instruction(string header, IEnumerable<InstructionArgument> arguments, IEnumerable<InstructionAction> actions,
        bool hasBreakpoint = false)
    {
        Header = header;
        AddArguments(arguments);
        AddActions(actions);
        HasBreakpoint = hasBreakpoint;
    }

    private void AddArguments(IEnumerable<InstructionArgument> arguments)
    {
        foreach (var argument in arguments)
        {
            Arguments.Add(argument);
        }
    }
    private void AddActions(IEnumerable<InstructionAction> actions)
    {
        foreach (var action in actions)
        {
            Actions.Add(action);
        }
    }

    public static bool operator ==(Instruction a, Instruction b)
    {
        return a.Header.ToLower().Trim() == b.Header.ToLower().Trim() && a.Arguments == b.Arguments && a.Actions == b.Actions &&
            a.ScriptLine == b.ScriptLine && a.HasBreakpoint == b.HasBreakpoint;
    }
    public static bool operator !=(Instruction a, Instruction b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        StringBuilder insText = new StringBuilder(Header);

        if (Arguments.Count > 0)
        {
            insText.Append($" : [{Arguments[0]}]");

            for (int i = 1; i < Arguments.Count; i++)
            {
                insText.Append($" , [{Arguments[i].ToString()}]");
            }
        }

        return insText.ToString();
    }
    
    public bool Equals(Instruction other)
    {
        return Header == other.Header && Arguments.Equals(other.Arguments) && Actions.Equals(other.Actions) && ScriptLine == other.ScriptLine && HasBreakpoint == other.HasBreakpoint;
    }
    public override bool Equals(object? obj)
    {
        return obj is Instruction other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Header, Arguments, Actions, ScriptLine, HasBreakpoint);
    }
}