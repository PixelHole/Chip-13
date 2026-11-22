using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.Computer.File.Components;

public class RegisterSaveInstance
{
    public string Name { get; set; }
    public string Id { get; set; }
    public long InitialValue { get; set; }
    public byte LengthLimit { get; set; }


    [JsonConstructor]
    public RegisterSaveInstance(string name, string id, long initialValue, byte lengthLimit)
    {
        Name = name;
        Id = id;
        InitialValue = initialValue;
        LengthLimit = lengthLimit;
    }
    public RegisterSaveInstance(Register register)
    {
        Name = register.Name;
        Id = register.Id;
        InitialValue = register.DefaultValue;
        LengthLimit = register.LengthLimit;
    }

    public Register ToRegister()
    {
        return new Register(Name, Id, InitialValue, InitialValue, LengthLimit);
    }
}