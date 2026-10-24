using System.Text.Json.Serialization;
using System.Windows.Media;

namespace Chippie_Lite_WPF.Computer.File.DefaultStates;

public class ColorsSaveInstance
{
    public ColorSaveInstance[] Colors { get; set; }


    [JsonConstructor]
    public ColorsSaveInstance(ColorSaveInstance[] colors)
    {
        Colors = colors;
    }
}