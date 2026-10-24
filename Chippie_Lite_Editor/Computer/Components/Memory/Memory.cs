namespace Chippie_Lite_WPF.Computer.Components;

public class Memory : ChippieComponent
{
    private readonly int BlockAddRange = 3;
    private List<MemoryBlock> RuntimeStorage { get; set; } = [];
    private List<MemoryBlock> InitialStorage { get; set; } = [];
    public bool ReadOnly { get; private set; }

    public int Size { get; set; } = 8192;
    public byte LengthLimit { get; set; } = 32;
    
    private long Mask
    {
        get
        {
            var mask = (long)Math.Pow(2, LengthLimit);
            return LengthLimit == 64 ? mask : mask - 1;
        }
    }

    private readonly Semaphore accessPool = new Semaphore(1, 1);

    public delegate void StorageUpdateAction(int index, long data);
    public event StorageUpdateAction? OnStorageUpdated;
    
    public delegate void InitialStorageUpdateAction(int index);
    public event InitialStorageUpdateAction? OnInitialStorageUpdated;


    internal void ResetRuntimeStorage()
    {
        RuntimeStorage.Clear();
    }
    internal void CopyInitialStorageToRuntime()
    {
        foreach (var block in RuntimeStorage)
        {
            for (int i = block.StartIndex; i < block.EndIndex; i++)
            {
                OnStorageUpdated?.Invoke(i, 0);
            }
        }
        
        ResetRuntimeStorage();
        
        foreach (var block in InitialStorage)
        {
            RuntimeStorage.Add(block);
            for (int i = block.StartIndex; i < block.EndIndex; i++)
            {
                OnStorageUpdated?.Invoke(i, block.Read(i));
            }
        }
    }

    public void Write(int index, long data)
    {
        if (WriteCall(index, data, RuntimeStorage)) OnStorageUpdated?.Invoke(index, data);
    }
    public long Read(int index)
    {
        return ReadCall(index, RuntimeStorage);
    }

    public void WriteInitial(int index, long data)
    {
        if (WriteCall(index, data, InitialStorage)) OnInitialStorageUpdated?.Invoke(index);
    }
    public long ReadInitial(int index)
    {
        return ReadCall(index, InitialStorage);
    }

    private bool WriteCall(int index, long data, List<MemoryBlock> source)
    {
        if (IsIndexOutOfHardRange(index) || ReadOnly) return false;
        
        accessPool.WaitOne();
        
        WriteToStorage(index, data, source);

        accessPool.Release();

        return true;
    }
    private long ReadCall(int index, List<MemoryBlock> source)
    {
        if (IsIndexOutOfHardRange(index)) return 0;
        
        accessPool.WaitOne();

        long data = ReadFromStorage(index, source);

        accessPool.Release();
        
        return data;
    }
    
    private void WriteToStorage(int index, long data, List<MemoryBlock> storage)
    {
        data = MaskData(data);
        var block = GetBlock(storage, index, BlockAddRange);
        if (block == null)
        {
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
    private long ReadFromStorage(int index, List<MemoryBlock> storage)
    {
        var block = GetBlock(storage, index, 0);

        if (block == null) return 0;

        return block.Read(index);
    }

    private void ResolveBlockConflict(List<MemoryBlock> storage, MemoryBlock block)
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

    private MemoryBlock? GetBlock(List<MemoryBlock> storage, int memoryIndex, int offset)
    {
        return storage.Find(block => block.IsIndexInRange(memoryIndex, offset));
    }

    internal MemoryBlock[] GetAllInitialBlocks()
    {
        MemoryBlock[] blocks = new MemoryBlock[InitialStorage.Count];

        for (int i = 0; i < blocks.Length; i++)
        {
            var memBlock = InitialStorage[i];
            blocks[i] = new MemoryBlock(memBlock.StartIndex, memBlock.Data);
        }

        return blocks;
    }
    internal void LoadInitialBlocks(MemoryBlock[] blocks)
    {
        InitialStorage.Clear();
        foreach (var block in blocks)
        {
            InitialStorage.Add(block);
        }
    }

    private long MaskData(long data)
    {
        return LengthLimit == 64 ? data : data & Mask;
    }

    private bool IsIndexInRange(int index) => index >= 0 && index < Size;
    private bool IsIndexOutOfHardRange(int index) => index < 0 || index >= Size;
}