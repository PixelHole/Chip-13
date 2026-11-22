using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.File.Components;

public class EnvironmentConfig
{
    public IOSaveInstance? IOData { get; }
    public MemorySaveInstance[]? MemoryModules { get; }
    public InstructionSetInstance? InstructionSet { get; }
    public RegisterSaveInstance[]? Registers { get; }
    public ColorsSaveInstance? Colors { get; }


    [JsonConstructor]
    public EnvironmentConfig(IOSaveInstance? ioData,
        MemorySaveInstance[]? memoryModules,
        InstructionSetInstance? instructionSet,
        RegisterSaveInstance[]? registers,
        ColorsSaveInstance? colors)
    {
        IOData = ioData;
        MemoryModules = memoryModules;
        InstructionSet = instructionSet;
        Registers = registers;
        Colors = colors;
    }
}