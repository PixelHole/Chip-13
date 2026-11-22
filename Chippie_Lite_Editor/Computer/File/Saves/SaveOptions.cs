using System.ComponentModel;

namespace Chippie_Lite_WPF.Computer.File;

public class SaveOptions
{
    public enum SaveTargets
    {
        Colors,
        InstructionSet,
        Memory, 
        IO,
        Registers,
    }

    public SaveTargets[] Targets { get; private set; } = [];

    public int Length => Targets.Length;

    public static readonly SaveOptions FullOptions = new([
        SaveTargets.Colors,
        SaveTargets.InstructionSet,
        SaveTargets.Memory,
        SaveTargets.IO,
        SaveTargets.Registers
    ]);
    public static readonly SaveOptions InstructionSetOptions = new([
        SaveTargets.InstructionSet,
    ]);
    public static readonly SaveOptions DeviceConfigOptions = new([
        SaveTargets.Colors,
        SaveTargets.Memory,
        SaveTargets.IO,
        SaveTargets.Registers
    ]);
    public static readonly SaveOptions NoOptions = new();
    
    public SaveOptions()
    {
    }
    public SaveOptions(SaveTargets[] targets)
    {
        Targets = targets;
    }

    public bool HasFlag(SaveTargets target) => Targets.Contains(target);
}