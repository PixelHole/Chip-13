using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.File.Components;

public class ColorsSaveInstance
{
    public ColorSaveInstance[] Colors { get; set; }


    [JsonConstructor]
    public ColorsSaveInstance(ColorSaveInstance[] colors)
    {
        Colors = colors;
    }
}