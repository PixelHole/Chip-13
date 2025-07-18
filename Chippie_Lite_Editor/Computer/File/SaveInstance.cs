using System.Text.Json;
using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.DefaultStates;

namespace Chippie_Lite_WPF.Computer.File;

public class SaveInstance
{
    public string Code { get; set; } = "";
    public RegisterSaveInstance[] Registers { get; init; } = [];
    public MemoryBlockSaveInstance[] MemoryBlocks { get; init; } = [];


    [JsonConstructor]
    public SaveInstance(string code, RegisterSaveInstance[] registers, MemoryBlockSaveInstance[] memoryBlocks)
    {
        Code = code;
        Registers = registers;
        MemoryBlocks = memoryBlocks;
    }
    public SaveInstance() { }
    
    
    public static string Serialize(SaveInstance save)
    {
        return JsonSerializer.Serialize(save);
    }
    public static SaveInstance? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<SaveInstance>(json);
    }
}