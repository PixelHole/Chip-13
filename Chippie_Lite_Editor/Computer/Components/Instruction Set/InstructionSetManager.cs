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

    public static bool HasSet(InstructionSet set) => LoadedSets.Contains(set);
    public static int IndexOf(InstructionSet set) => LoadedSets.IndexOf(set);
}