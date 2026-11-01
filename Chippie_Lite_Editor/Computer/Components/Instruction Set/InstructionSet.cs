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
    
    
    public override int GetHashCode()
    {
        return Templates.GetHashCode();
    }
}