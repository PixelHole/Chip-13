using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.Computer.File.DefaultStates;

public class MemorySaveInstance
{
    public int MemorySize { get; }
    public byte LengthLimit { get; }
    public bool ReadOnly { get; }
    public bool Permanent { get; }
    public int BlockAddRange { get; }
    public MemoryBlockSaveInstance[] Blocks { get; }


    [JsonConstructor]
    public MemorySaveInstance(int memorySize, byte lengthLimit, bool readOnly, bool permanent, int blockAddRange, MemoryBlockSaveInstance[] blocks)
    {
        MemorySize = memorySize;
        LengthLimit = lengthLimit;
        ReadOnly = readOnly;
        Permanent = permanent;
        BlockAddRange = blockAddRange;
        Blocks = blocks;
    }

    public Memory ToMemory()
    {
        Memory memory = new Memory(BlockAddRange, ReadOnly, Permanent, MemorySize, LengthLimit);
        
        MemoryBlock[] blocks = new MemoryBlock[Blocks.Length];

        for (int i = 0; i < Blocks.Length; i++)
        {
            blocks[i] = Blocks[i].ToBlock();
        }

        memory.LoadInitialBlocks(blocks);

        return memory;
    }
}