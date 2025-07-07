namespace Chippie_Lite_WPF.Computer.Components;

public static class Memory
{
    private static int[] Storage { get; set; } = new int[8192];
    public static bool ReadOnly { get; private set; }

    private static readonly Semaphore accessPool = new Semaphore(1, 1);

    public delegate void StorageUpdateAction(int start, int end);
    public static event StorageUpdateAction OnStorageUpdated;

    public static int Read(int index)
    {
        accessPool.WaitOne();
        
        int result = Storage[WrapIndex(index)];

        accessPool.Release();

        return result;
    }
    public static void Write(int content, int index)
    {
        accessPool.WaitOne();

        Storage[WrapIndex(index)] = content;

        accessPool.Release();
        
        OnStorageUpdated?.Invoke(WrapIndex(index), WrapIndex(index));
    }

    public static int[] ReadRange(int start, int count)
    {
        int[] result = new int[count];
        
        accessPool.WaitOne();

        for (int i = 0; i < count; i++)
        {
            result[i] = Storage[WrapIndex(start + i)];
        }

        accessPool.Release();

        return result;
    }
    public static void WriteRange(int[] content, int index)
    {
        accessPool.WaitOne();

        for (int i = 0; i < content.Length; i++)
        {
            Storage[WrapIndex(index + i)] = content[i];
        }

        accessPool.Release();
        
        OnStorageUpdated?.Invoke(WrapIndex(index), WrapIndex(index + content.Length));
    }

    private static int WrapIndex(int index)
    {
        if (index >= Storage.Length) return index % Storage.Length;
        if (index < 0) return (Storage.Length) - -index % Storage.Length;
        return index;
    }
}