using System.Text.Json.Serialization;
using Chippie_Lite_WPF.UI.ConsoleControl.Units;

namespace Chippie_Lite_WPF.Computer.File.Components;

public struct IOSaveInstance
{
    public Vector2Int DisplaySize { get; }


    [JsonConstructor]
    public IOSaveInstance(Vector2Int displaySize)
    {
        DisplaySize = displaySize;
    }
}