using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.Computer;

public static class ChippieComponentManager
{
    public static List<ChippieComponent> Components { get; } = [];


    public static void RemoveAllComponentsOfType<T>() where T : ChippieComponent
    {
        var comps = GetComponentsOfType<T>();
        foreach (var comp in comps)
        {
            Components.Remove(comp);
        }
    }
    
    public static void AddComponent(ChippieComponent component)
    {
        Components.Add(component);
    }
    public static bool RemoveComponent(ChippieComponent component)
    {
        return Components.Remove(component);
    }

    public static T? GetComponentOfType<T>() where T : class
    {
        return Components.Find(component => component.GetType() == typeof(T)) as T;
    }
    public static List<T> GetComponentsOfType<T>() where T : class
    {
        var matches = Components.FindAll(component => component.GetType() == typeof(T)) as List<T>;
        return matches ?? [];
    }

    internal static void ResetNonPermanentMemoryModules()
    {
        foreach (var memory in GetComponentsOfType<Memory>().Where(memory => !memory.Permanent))
        {
            memory.ResetToInitialStorage();
        }
    }
}