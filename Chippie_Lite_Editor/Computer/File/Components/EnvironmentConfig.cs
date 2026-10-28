using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using wpf_Console;

namespace Chippie_Lite_WPF.Computer.File.DefaultStates;

public class EnvironmentConfig
{
    public IOSaveInstance IOData { get; }
    public MemorySaveInstance[] MemoryModules { get; }
    public InstructionTemplate[] InstructionSet { get; }
    public RegisterSaveInstance[] Registers { get; }
    public ColorsSaveInstance Colors { get; }


    [JsonConstructor]
    public EnvironmentConfig(IOSaveInstance ioData,
        MemorySaveInstance[] memoryModules,
        InstructionTemplate[] instructionSet,
        RegisterSaveInstance[] registers,
        ColorsSaveInstance colors)
    {
        IOData = ioData;
        MemoryModules = memoryModules;
        InstructionSet = instructionSet;
        Registers = registers;
        Colors = colors;
    }
}