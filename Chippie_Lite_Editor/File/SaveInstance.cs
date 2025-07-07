using System.Text.Json;
using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Instance.DefaultStates;

namespace Chippie_Lite_WPF.Instance;

public class SaveInstance
{
    public string Assembly { get; set; }
    public RegisterSaveInstance[] Registers { get; set; }
    public MemoryBlockSaveInstance[] MemoryBlocks { get; set; }


    [JsonConstructor]
    public SaveInstance(string assembly, RegisterSaveInstance[] registers, MemoryBlockSaveInstance[] memoryBlocks)
    {
        Assembly = assembly;
        Registers = registers;
        MemoryBlocks = memoryBlocks;
    }
    public SaveInstance() : this("", [], []) {}

    
    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }
    public SaveInstance? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<SaveInstance>(json);
    }
}