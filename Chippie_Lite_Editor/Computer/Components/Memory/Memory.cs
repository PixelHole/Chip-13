using System.Globalization;

namespace Chippie_Lite_WPF.Computer.Components;

public class Memory : ChippieComponent
{
    private string name = "Memory";
    private int size = 8192;
    private byte lengthLimit = 32;


    public string Name
    {
        get => name;
        set
        {
            if (name == value) return;
            name = value;
            OnNameChanged?.Invoke(this, name);
        }
    }
    
    internal readonly int BlockAddRange;
    private List<MemoryBlock> RuntimeStorage { get; set; } = [];
    private List<MemoryBlock> InitialStorage { get; set; } = [];
    public bool ReadOnly { get; set; }

    public int Size
    {
        get => size;
        set
        {
            if (size == value) return;
            size = value;
            OnSizeChanged?.Invoke(this, size);
        }
    }
    public byte LengthLimit
    {
        get => lengthLimit;
        set
        {
            if (lengthLimit == value) return;
            lengthLimit = value;
            OnLengthLimitChanged?.Invoke(this, lengthLimit);
        }
    }
    public bool Permanent { get; set; } = false;

    public double SizeInBytes => Size * LengthLimit / 8d;
    
    private long Mask
    {
        get
        {
            var mask = (long)Math.Pow(2, LengthLimit);
            return LengthLimit == 64 ? mask : mask - 1;
        }
    }

    private readonly Semaphore accessPool = new Semaphore(1, 1);

    public delegate void NameChangeAction(Memory sender, string name);
    public delegate void SizeChangeAction(Memory sender, int size);
    public delegate void LengthLimitChangeAction(Memory sender, byte lengthLimit);
    public delegate void StorageUpdateAction(int index, long data);
    public delegate void InitialStorageUpdateAction(int index);
    
    public event StorageUpdateAction? OnStorageUpdated;
    public event InitialStorageUpdateAction? OnInitialStorageUpdated;
    public event NameChangeAction? OnNameChanged;
    public event SizeChangeAction? OnSizeChanged;
    public event LengthLimitChangeAction? OnLengthLimitChanged;


    public Memory(string name = "Memory", int blockAddRange = 3, bool readOnly = false, bool permanent = false, int size = 8192, byte lengthLimit = 32)
    {
        Name = name;
        BlockAddRange = blockAddRange;
        ReadOnly = readOnly;
        Permanent = permanent;
        Size = size;
        LengthLimit = lengthLimit;
    }

    internal void ResetRuntimeStorage()
    {
        RuntimeStorage.Clear();
    }
    internal void ResetToInitialStorage()
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

    public Memory Clone()
    {
        var clone = new Memory(Name, BlockAddRange, ReadOnly, Permanent, Size, LengthLimit);
        clone.LoadInitialBlocks(GetAllInitialBlocks());
        return clone;
    }
    
    public string GetSizeString()
    {
        int index = 0;
        string[] labels = ["Bs", "KBs", "MBs", "GBs"];
        double size = SizeInBytes;
        while (size > 1024)
        {
            index++;
            size /= 1024;
        }

        return size.ToString(CultureInfo.InvariantCulture) + labels[index];
    }

    protected bool Equals(Memory other)
    {
        return name == other.name &&
               size == other.size &&
               lengthLimit == other.lengthLimit &&
               BlockAddRange == other.BlockAddRange &&
               RuntimeStorage.SequenceEqual(other.RuntimeStorage) &&
               InitialStorage.SequenceEqual(other.InitialStorage) &&
               ReadOnly == other.ReadOnly &&
               Permanent == other.Permanent;
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Memory)obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(name, size, lengthLimit, BlockAddRange, RuntimeStorage, InitialStorage, ReadOnly, Permanent);
    }

    public static bool operator ==(Memory? a, Memory? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }
    public static bool operator !=(Memory? a, Memory? b)
    {
        return !(a == b);
    }
}