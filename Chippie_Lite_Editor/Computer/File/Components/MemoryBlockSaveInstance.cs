using System.Text.Json.Serialization;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.Computer.File.DefaultStates;

public class MemoryBlockSaveInstance
{
    public int StartIndex { get; set; }
    public long[] Data { get; set; }


    [JsonConstructor]
    public MemoryBlockSaveInstance(int startIndex, long[] data)
    {
        StartIndex = startIndex;
        Data = data;
    }
    public MemoryBlockSaveInstance(MemoryBlock block)
    {
        StartIndex = block.StartIndex;
        Data = block.Data.ToArray();
    }


    public MemoryBlock ToBlock()
    {
        return new MemoryBlock(StartIndex, Data);
    }
}