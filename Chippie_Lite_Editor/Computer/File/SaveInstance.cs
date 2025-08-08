using System.Text.Json;
using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.DefaultStates;

namespace Chippie_Lite_WPF.Computer.File;

public class SaveInstance
{
    public string Code { get; set; }
    public RegisterSaveInstance[] Registers { get; }
    public MemoryBlockSaveInstance[] MemoryBlocks { get; }
    public EnvironmentConfig Config { get; }


    [JsonConstructor]
    public SaveInstance(string code, RegisterSaveInstance[] registers, MemoryBlockSaveInstance[] memoryBlocks, EnvironmentConfig config)
    {
        Code = code;
        Registers = registers;
        MemoryBlocks = memoryBlocks;
        Config = config;
    }
}