using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.File.DefaultStates;

public class InstructionSetInstance
{
    public string Name { get; }
    public InstructionTemplate[] Templates { get; }


    [JsonConstructor]
    public InstructionSetInstance(string name, InstructionTemplate[] templates)
    {
        Name = name;
        Templates = templates;
    }
    public InstructionSetInstance(InstructionSet set)
    {
        Name = set.Name;
        Templates = set.GetTemplates();
    }

    public InstructionSet ToSet()
    {
        return new InstructionSet(Name, Templates);
    }

    public override int GetHashCode()
    {
        return Templates.GetHashCode();
    }
}