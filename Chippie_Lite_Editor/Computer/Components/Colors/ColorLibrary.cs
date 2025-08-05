using System.Windows.Media;

namespace Chippie_Lite_WPF.Computer.Components;

public static class ColorLibrary
{
    public static Color[] Colors { get; } = new[]
    {
        Color.FromRgb(0, 0, 0),
        Color.FromRgb(29, 43, 83),
        Color.FromRgb(126, 37, 83),
        Color.FromRgb(0, 135, 81),
        Color.FromRgb(171, 82, 54),
        Color.FromRgb(95, 87, 79),
        Color.FromRgb(194, 195, 199),
        Color.FromRgb(255, 241, 232),
        Color.FromRgb(255, 0, 77),
        Color.FromRgb(255, 163, 0),
        Color.FromRgb(255, 236, 39),
        Color.FromRgb(0, 228, 54),
        Color.FromRgb(41, 173, 255),
        Color.FromRgb(131, 118, 156),
        Color.FromRgb(255, 119, 168),
        Color.FromRgb(255, 204, 170),
    };

    public static Color GetColor(int index)
    {
        index = WrapIndex(index);

        return Colors[index];
    }

    private static int WrapIndex(int index)
    {
        if (index > Colors.Length) index %= Colors.Length;
        if (index < 0) index = Colors.Length - (-index % Colors.Length);
        return index;
    }
}