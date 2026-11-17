using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.Computer.Components;

public static class ColorLibrary
{
    private static List<Color> Colors { get; set; } = [];
    public static int Count => Colors.Count;

    public delegate void ColorAddedAction(Color color, int index);
    public static event ColorAddedAction? OnColorAdded;
    
    public delegate void ColorRemovedAction(Color color, int index);
    public static event ColorRemovedAction? OnColorRemoved;
    
    public delegate void ColorUpdatedAction(Color color, int index);
    public static event ColorUpdatedAction? OnColorUpdated;


    public static void Clear() => Colors.Clear();
    
    public static void InsertColor(Color color, int index)
    {
        Colors.Insert(index, color);
        OnColorAdded?.Invoke(color, index);
    }
    public static void AddColor(Color color)
    {
        Colors.Add(color);
        OnColorAdded?.Invoke(color, Colors.Count - 1);
    }

    public static bool RemoveColor(Color color)
    {
        int index = Colors.IndexOf(color);
        if (index == -1) return false;
        Colors.Remove(color);
        OnColorRemoved?.Invoke(color, index);
        return true;
    }

    public static Color GetColor(int index)
    {
        index = IndexUtility.WrapIndex(index, Colors.Count);

        return Colors[index];
    }
    public static void SetColor(Color color, int index)
    {
        Colors[index] = color;
        OnColorUpdated?.Invoke(color, index);
    }

    public static Color[] GetColors() => Colors.ToArray();
    public static void LoadColors(IEnumerable<Color> colors)
    {
        Clear();
        foreach (var color in colors)
        {
            AddColor(color);
        }
    }
}