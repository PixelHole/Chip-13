namespace Chippie_Lite_WPF.Computer.Components;

public static class Memory
{
    private static readonly int BlockAddRange = 3;
    private static List<MemoryBlock> RuntimeStorage { get; set; } = [];
    private static List<MemoryBlock> InitialStorage { get; set; } = [];
    public static bool ReadOnly { get; private set; }

    public static readonly int Size = 8192;

    private static readonly Semaphore accessPool = new Semaphore(1, 1);

    public delegate void StorageUpdateAction(int index);
    public static event StorageUpdateAction? OnStorageUpdated;
    
    public delegate void InitialStorageUpdateAction(int index);
    public static event InitialStorageUpdateAction? OnInitialStorageUpdated;


    internal static void ResetRuntimeStorage()
    {
        RuntimeStorage.Clear();
    }
    internal static void CopyInitialStorageToRuntime()
    {
        ResetRuntimeStorage();
        foreach (var block in InitialStorage)
        {
            RuntimeStorage.Add(block);
        }
    }

    public static void Write(int index, int data)
    {
        accessPool.WaitOne();
        
        WriteToStorage(index, data, RuntimeStorage);

        accessPool.Release();
        
        OnStorageUpdated?.Invoke(index);
    }
    public static int Read(int index)
    {
        accessPool.WaitOne();

        int data = ReadFromStorage(index, RuntimeStorage);

        accessPool.Release();
        
        return data;
    }

    public static void WriteInitial(int index, int data)
    {
        accessPool.WaitOne();
        
        WriteToStorage(index, data, InitialStorage);

        accessPool.Release();
        
        OnStorageUpdated?.Invoke(index);
    }
    public static int ReadInitial(int index)
    {
        accessPool.WaitOne();

        int data = ReadFromStorage(index, InitialStorage);

        accessPool.Release();
        
        return data;
    }

    private static void WriteToStorage(int index, int data, List<MemoryBlock> storage)
    {
        var block = GetBlock(storage, index, BlockAddRange);
        if (block == null)
        {
            if (data == 0) return;
            block = new MemoryBlock(index, [data]);
            storage.Add(block);
        }
        else
        {
            block.InsertData(index, data);
            ResolveBlockConflict(storage, block);
            if (block.FilledCells <= 0) storage.Remove(block);
        }
    }
    private static int ReadFromStorage(int index, List<MemoryBlock> storage)
    {
        var block = GetBlock(storage, index, 0);

        if (block == null) return 0;

        return block.Read(index);
    }

    private static void ResolveBlockConflict(List<MemoryBlock> storage, MemoryBlock block)
    {
        var endConflict = GetBlock(storage, block.EndIndex, BlockAddRange);
        
        if (endConflict != null && endConflict != block)
        {
            block.MergeWithBlock(endConflict);
            storage.Remove(endConflict);
        }

        var startConflict = GetBlock(storage, block.StartIndex, BlockAddRange);
        
        if (startConflict != null && startConflict != block)
        {
            block.MergeWithBlock(startConflict);
            storage.Remove(startConflict);
        }
    }

    private static MemoryBlock? GetBlock(List<MemoryBlock> storage, int memoryIndex, int offset)
    {
        return storage.Find(block => block.IsIndexInRange(memoryIndex, offset));
    }

    internal static MemoryBlock[] GetAllInitialBlocks()
    {
        MemoryBlock[] blocks = new MemoryBlock[InitialStorage.Count];

        for (int i = 0; i < blocks.Length; i++)
        {
            var memBlock = InitialStorage[i];
            blocks[i] = new MemoryBlock(memBlock.StartIndex, memBlock.Data);
        }

        return blocks;
    }
    internal static void LoadInitialBlocks(MemoryBlock[] blocks)
    {
        InitialStorage.Clear();
        foreach (var block in blocks)
        {
            InitialStorage.Add(block);
        }
    }
}