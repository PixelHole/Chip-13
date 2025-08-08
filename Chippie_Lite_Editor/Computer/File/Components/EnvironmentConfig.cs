using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions.Templates;
using wpf_Console;

namespace Chippie_Lite_WPF.Computer.File.DefaultStates;

public class EnvironmentConfig
{
    public Vector2Int DisplaySize { get; }
    public int MemorySize { get; }
    public InstructionTemplate[] AddedInstructions { get; }
    public Register[] AddedRegisters { get; }


    public EnvironmentConfig(Vector2Int displaySize, int memorySize, InstructionTemplate[] addedInstructions, Register[] addedRegisters)
    {
        DisplaySize = displaySize;
        MemorySize = memorySize;
        AddedInstructions = addedInstructions;
        AddedRegisters = addedRegisters;
    }
}