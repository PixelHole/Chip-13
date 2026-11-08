using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.Instructions;

public struct InstructionAction : IEquatable<InstructionAction>
{
    [JsonIgnore]
    public Instruction Instruction { get; private set; }
    public string Header { get; }
    public string Condition { get; }
    public string[] Arguments { get; }


    [JsonConstructor]
    public InstructionAction(string header, string[] arguments, string condition = "")
    {
        Header = header;
        Condition = condition;
        Arguments = arguments;
    }

    public void SetInstruction(Instruction ins) => Instruction = ins;

    public bool Equals(InstructionAction other)
    {
        return Header == other.Header && Condition == other.Condition && Arguments.SequenceEqual(other.Arguments);
    }
    public override bool Equals(object? obj)
    {
        return obj is InstructionAction other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Instruction, Header, Condition, Arguments);
    }
}