using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components;

public static class ColorLibrary
{
    public static List<Color> Colors { get; set; } = [];


    public static void Clear() => Colors.Clear();
    public static void AddColor(Color color) => Colors.Add(color);
    
    public static Color GetColor(int index)
    {
        index = IndexUtility.WrapIndex(index, Colors.Count);

        return Colors[index];
    }
}