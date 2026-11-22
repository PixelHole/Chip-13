using System.Text.Json.Serialization;
using System.Windows.Media;

namespace Chippie_Lite_WPF.Computer.File.Components;

public class ColorSaveInstance
{
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    public byte Alpha { get; set; }

    
    [JsonConstructor]
    public ColorSaveInstance(byte red, byte green, byte blue, byte alpha)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }
    public ColorSaveInstance(Color color)
    {
        Red = color.R;
        Green = color.G;
        Blue = color.B;
        Alpha = color.A;
    }
    
    public Color ToColor() => Color.FromArgb(Alpha, Red, Green, Blue);
}