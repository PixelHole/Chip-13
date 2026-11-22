using System.Text.Json;
using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.Components;

namespace Chippie_Lite_WPF.Computer.File;

public class SaveInstance
{
    public string Code { get; set; }
    public EnvironmentConfig Config { get; }


    [JsonConstructor]
    public SaveInstance(string code, EnvironmentConfig config)
    {
        Code = code;
        Config = config;
    }
}