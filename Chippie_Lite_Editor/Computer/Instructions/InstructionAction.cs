using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.Instructions;

public struct InstructionAction : IEquatable<InstructionAction>
{
    [JsonIgnore]
    public Instruction Instruction { get; set; }
    public string Header { get; private set; }
    public string Condition { get; private set; }
    public string[] Arguments { get; private set; }


    [JsonConstructor]
    public InstructionAction(string header, string condition, string[] arguments)
    {
        Header = header;
        Condition = condition;
        Arguments = arguments;
    }
    [JsonConstructor]
    public InstructionAction(string header, string[] arguments)
    {
        Header = header;
        Condition = "";
        Arguments = arguments;
    }

    public void SetInstruction(Instruction ins) => Instruction = ins;
    
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is InstructionAction other) return Equals(other);
        return base.Equals(obj);
    }
    public bool Equals(InstructionAction other)
    {
        return Header == other.Header && Arguments.Equals(other.Arguments);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Header, Arguments);
    }
}