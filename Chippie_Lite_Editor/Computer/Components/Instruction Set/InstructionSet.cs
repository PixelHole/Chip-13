using System.Text;
using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public class InstructionSet
{
    public string Name { get; set; }
    private List<InstructionTemplate> Templates { get; } = [];
    public bool ReadOnly { get; set; }
    public int Count => Templates.Count;

    public delegate void TemplateAddedAction(InstructionSet set, InstructionTemplate template, int index);
    public delegate void TemplateRemovedAction(InstructionSet set, InstructionTemplate template, int index);

    public event TemplateAddedAction? OnTemplateAdded;
    public event TemplateRemovedAction? OnTemplateRemoved;

    [JsonConstructor]
    public InstructionSet(string name, IEnumerable<InstructionTemplate> templates, bool readOnly)
    {
        Name = name;
        AddTemplates(templates);
        ReadOnly = readOnly;
    }
    public InstructionSet(string name, IEnumerable<InstructionTemplate> templates)
    {
        Name = name;
        AddTemplates(templates);
        ReadOnly = false;
    }

    public void FlushTemplates() => Templates.Clear();
    public void AddTemplates(IEnumerable<InstructionTemplate> templates)
    {
        foreach (var template in templates)
        {
            Templates.Add(template);
        }
    }
    public bool AddTemplate(InstructionTemplate template)
    {
        if (FindTemplate(t => t.Name == template.Name || t.Header == template.Header) != null) return false;
        Templates.Add(template);
        OnTemplateAdded?.Invoke(this, template, Count - 1);
        return true;
    }
    public bool RemoveTemplate(InstructionTemplate template)
    {
        var match = FindTemplate(t => t.Name == template.Name || t.Header == template.Header);
        if (match == null) return false;
        int index = Templates.IndexOf(match);
        Templates.Remove(template);
        OnTemplateRemoved?.Invoke(this, template, index);
        return true;
    }
    
    public bool HasTemplateWithName(string name) => FindTemplate(t => t.Name == name) != null;
    public bool HasTemplateWithHeader(string header) => FindTemplate(t => t.Header == header) != null;

    public InstructionTemplate GetTemplate(int index) => Templates[IndexUtility.WrapIndex(index, Templates.Count)];
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

    protected bool Equals(InstructionSet? other)
    {
        return other is not null && Templates.SequenceEqual(other.Templates) && string.Equals(Name, other.Name, StringComparison.Ordinal);
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

    public static bool operator ==(InstructionSet? a, InstructionSet? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }
    public static bool operator !=(InstructionSet? a, InstructionSet? b)
    {
        return !(a == b);
    }
}