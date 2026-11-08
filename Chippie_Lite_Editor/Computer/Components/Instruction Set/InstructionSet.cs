using System.Text;
using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components;

public class InstructionSet
{
    public string Name { get; }
    private List<InstructionTemplate> Templates { get; } = [];


    public InstructionSet(string name, IEnumerable<InstructionTemplate> templates)
    {
        Name = name;
        AddTemplates(templates);
    }

    public void FlushTemplates() => Templates.Clear();
    public void AddTemplates(IEnumerable<InstructionTemplate> templates)
    {
        foreach (var template in templates)
        {
            Templates.Add(template);
        }
    }
    
    public InstructionTemplate[] GetTemplates()
    {
        return Templates.ToArray();
    }

    public InstructionTemplate? FindTemplate(string header)
    {
        return FindTemplate(temp => temp.Header == header);
    }
    public InstructionTemplate? FindTemplate(Predicate<InstructionTemplate> predicate)
    {
        return Templates.Find(predicate);
    }

    public string GetHeadersCsv()
    {
        StringBuilder csv = new StringBuilder();

        for (int i = 0; i < Templates.Count; i++)
        {
            string header = Templates[i].Header;

            csv.Append(header);

            if (i < Templates.Count - 1 && !string.IsNullOrEmpty(header)) csv.Append(',');
        }

        return csv.ToString();
    }

    protected bool Equals(InstructionSet other)
    {
        return Templates.SequenceEqual(other.Templates);
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((InstructionSet)obj);
    }
    public override int GetHashCode()
    {
        return Templates.GetHashCode();
    }
}