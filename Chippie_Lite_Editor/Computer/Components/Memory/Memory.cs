namespace Chippie_Lite_WPF.Computer.Components;

public static class Memory
{
    private static readonly int BlockAddRange = 3;
    private static List<MemoryBlock> Storage { get; set; } = [];
    public static bool ReadOnly { get; private set; }

    public static readonly int Size = 8192;

    private static readonly Semaphore accessPool = new Semaphore(1, 1);

    public delegate void StorageUpdateAction(int index);
    public static event StorageUpdateAction? OnStorageUpdated;


    public static void Write(int index, int data)
    {
        accessPool.WaitOne();
        
        var block = GetBlock(index, BlockAddRange);
        if (block == null)
        {
            block = new MemoryBlock(index, [data]);
            Storage.Add(block);
        }
        else
        {
            block.InsertData(index, data);
            ResolveBlockConflict(block);
        }

        accessPool.Release();
        
        OnStorageUpdated?.Invoke(index);
    }
    public static int Read(int index)
    {
        accessPool.WaitOne();

        var block = GetBlock(index, 0);
        
        int data = block == null ? 0 : block.Data[index - block.StartIndex];

        accessPool.Release();
        
        return data;
    }

    private static void ResolveBlockConflict(MemoryBlock block)
    {
        var endConflict = GetBlock(block.EndIndex, BlockAddRange);
        
        if (endConflict != null)
        {
            block.MergeWithBlock(endConflict);
            Storage.Remove(endConflict);
        }

        var startConflict = GetBlock(block.StartIndex, BlockAddRange);
        
        if (startConflict != null)
        {
            block.MergeWithBlock(startConflict);
            Storage.Remove(startConflict);
        }
    }

    private static MemoryBlock? GetBlock(int memoryIndex, int offset)
    {
        return Storage.Find(block => block.IsIndexInRange(memoryIndex, offset));
    }

    internal static void AddBlock(MemoryBlock block)
    {
        Storage.Add(block);
    }

    public static MemoryBlock[] GetAllBlocks()
    {
        MemoryBlock[] blocks = new MemoryBlock[Storage.Count];

        for (int i = 0; i < blocks.Length; i++)
        {
            var memBlock = Storage[i];
            blocks[i] = new MemoryBlock(memBlock.StartIndex, memBlock.Data);
        }

        return blocks;
    }
}