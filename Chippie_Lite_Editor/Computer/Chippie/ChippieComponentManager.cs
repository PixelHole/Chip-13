using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer;

public static class ChippieComponentManager
{
    public static List<ChippieComponent> Components { get; } = [];

    public delegate void ComponentAddedAction(ChippieComponent component, int index);
    public delegate void ComponentRemovedAction(ChippieComponent component, int index); 
    public static event ComponentAddedAction? OnComponentAdded;
    public static event ComponentRemovedAction? OnComponentRemoved;

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
        OnComponentAdded?.Invoke(component, Components.Count - 1);
    }
    public static bool RemoveComponent(ChippieComponent component)
    {
        int index = Components.IndexOf(component);
        if (index == -1) return false;
        Components.Remove(component);
        OnComponentAdded?.Invoke(component, index);
        return true;
    }

    public static T? GetComponentOfType<T>() where T : class
    {
        return Components.Find(component => component.GetType() == typeof(T)) as T;
    }
    public static List<T> GetComponentsOfType<T>() where T : class
    {
        return Components.FindAll(component => component.GetType() == typeof(T)).ConvertAll(input => input as T)!;
    }

    public static T? GetNthComponentOfType<T>(int index) where T : class
    {
        var components = GetComponentsOfType<T>();
        return components.Count == 0 ? null : components[IndexUtility.WrapIndex(index, components.Count)];
    }

    internal static void ResetNonPermanentMemoryModules()
    {
        foreach (var memory in GetComponentsOfType<Memory>().Where(memory => !memory.Permanent))
        {
            memory.ResetToInitialStorage();
        }
    }
}