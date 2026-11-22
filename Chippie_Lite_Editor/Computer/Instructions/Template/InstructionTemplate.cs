using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.Instructions;

public class InstructionTemplate
{
    public string Name { get; set; }
    public string Header { get; set; }
    public InstructionArgumentType[] ArgumentTypes { get; set; }
    public InstructionAction[] Actions { get; set; }


    
    [JsonConstructor]
    public InstructionTemplate(string name, string header, InstructionArgumentType[] argumentTypes, InstructionAction[] actions)
    {
        Name = name;
        Header = header;
        ArgumentTypes = argumentTypes;
        Actions = actions;
    }

    public override string ToString()
    {
        return $"{Name} [{Header}]";
    }

    protected bool Equals(InstructionTemplate other)
    {
        return Name == other.Name && Header == other.Header && ArgumentTypes.SequenceEqual(other.ArgumentTypes) && Actions.SequenceEqual(other.Actions);
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((InstructionTemplate)obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Header, ArgumentTypes, Actions);
    }
}