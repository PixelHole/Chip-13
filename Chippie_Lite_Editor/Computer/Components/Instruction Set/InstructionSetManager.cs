using System.ComponentModel;
using System.Windows.Forms;
using Chippie_Lite_WPF.Computer.File;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Instruction_Sets;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionSetManager
{
    private static int activeIndex = 0;
    
    private static List<InstructionSet> LoadedSets { get; } = [];
    public static int Count => LoadedSets.Count;
    public static int ActiveSetIndex
    {
        get => activeIndex;
        set
        {
            if (activeIndex == value) return;
            activeIndex = value;
            OnActiveSetChanged?.Invoke(ActiveInstructionSet, value);
        } 
    }
    public static InstructionSet ActiveInstructionSet => LoadedSets[ActiveSetIndex];

    public delegate void SetLoadedAction(InstructionSet set, int index);
    public static event SetLoadedAction? OnSetLoaded;
    
    public delegate void SetUnloadedAction(InstructionSet set, int index);
    public static event SetUnloadedAction? OnSetUnloaded;

    public delegate void ActiveSetChangedAction(InstructionSet set, int index);
    public static event ActiveSetChangedAction? OnActiveSetChanged;

    public static InstructionSet DuplicateSet(InstructionSet set)
    {
        InstructionSet dupe = new InstructionSet(GenerateNewName(set.Name), set.GetTemplates(), false);
        return dupe;
    }
    public static void LoadSet(InstructionSet set)
    {
        LoadedSets.Add(set);
        OnSetLoaded?.Invoke(set, LoadedSets.Count - 1);
    }
    public static void UnloadSet(InstructionSet set)
    {
        int index = IndexOf(set);
        LoadedSets.Remove(set);
        OnSetUnloaded?.Invoke(set, index);
    }

    public static InstructionSet? FindWithName(string name) => LoadedSets.Find(set => set.Name == name);
    public static InstructionSet GetSet(int index) => LoadedSets[index];
    public static InstructionSet[] GetSets() => LoadedSets.ToArray();

    public static string[] GetSetNames()
    {
        string[] names = new string[Count];

        for (int i = 0; i < Count; i++)
        {
            names[i] = LoadedSets[i].Name;
        }

        return names;
    }

    public static void SetActive(string name)
    {
        var set = FindWithName(name);
        if (set == null) return;
        ActiveSetIndex = IndexOf(set);
    }
    public static void SetActive(InstructionSet set) => ActiveSetIndex = IndexOf(set);
    
    public static bool HasSet(InstructionSet set) => LoadedSets.Contains(set);
    public static int IndexOf(InstructionSet set) => LoadedSets.IndexOf(set);

    public static string GenerateNewName(string baseName = "")
    {
        string name = string.IsNullOrEmpty(baseName) ? "Instruction set" : baseName;
        if (FindWithName(name) == null) return name;
        for (int i = 1; i <= 1000; i++) if (FindWithName(name + i) == null) return name + i;
        throw new TooManyInstructionSetsException($"You have too many instructions that start with the name {name}. please delete some. for your own good");
    }
}