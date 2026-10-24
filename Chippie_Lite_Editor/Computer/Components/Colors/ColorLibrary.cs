using System.Windows.Media;

namespace Chippie_Lite_WPF.Computer.Components;

public static class ColorLibrary
{
    public static List<Color> Colors { get; set; } = [];


    public static void Clear() => Colors.Clear();
    public static void AddColor(Color color) => Colors.Add(color);
    
    public static Color GetColor(int index)
    {
        index = WrapIndex(index);

        return Colors[index];
    }

    private static int WrapIndex(int index)
    {
        if (index > Colors.Count) index %= Colors.Count;
        if (index < 0) index = Colors.Count - (-index % Colors.Count);
        return index;
    }
}