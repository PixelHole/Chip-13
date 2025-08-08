using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.Computer.File.DefaultStates;

public class RegisterSaveInstance
{
    public string Name { get; set; }
    public int DefaultValue { get; set; }


    [JsonConstructor]
    public RegisterSaveInstance(string name, int defaultValue)
    {
        Name = name;
        DefaultValue = defaultValue;
    }
    public RegisterSaveInstance(Register register)
    {
        Name = register.Name;
        DefaultValue = register.DefaultValue;
    }
}