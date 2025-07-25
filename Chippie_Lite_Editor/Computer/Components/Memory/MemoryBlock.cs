namespace Chippie_Lite_WPF.Computer.Components;

public class MemoryBlock
{
    public int StartIndex { get; private set; } = 0;
    public List<int> Data { get; } = [];
    public int EndIndex => StartIndex + Data.Count - 1;


    public MemoryBlock(int startIndex, IList<int> data)
    {
        StartIndex = startIndex;
        AddDataRange(startIndex, data);
    }

    public void AddDataRange(int memIndex, IList<int> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            var num = data[i];
            
            InsertData(memIndex + i, num);
        }
    }
    
    public void InsertData(int memIndex, int data)
    {
        if (IsIndexInRange(memIndex)) ReplaceData(memIndex, data);
        else if (memIndex < StartIndex) InsertDataToStart(memIndex, data);
        else InsertDataToEnd(memIndex, data);
    }
    private void ReplaceData(int memIndex, int data)
    {
        Data[MemoryToInternalIndex(memIndex)] = data;
    }
    private void InsertDataToEnd(int memIndex, int data)
    {
        int end = EndIndex;
        for (int i = end; i < memIndex; i++)
        {
            Data.Add(0);
        }

        Data.Add(data);
    }
    private void InsertDataToStart(int memIndex, int data)
    {
        Data.Insert(0, data);
        
        for (int i = memIndex; i < StartIndex; i++)
        {
            Data.Insert(0, 0);
        }

        if (StartIndex < memIndex) StartIndex = memIndex;
    }
    

    public void MergeWithBlock(MemoryBlock other)
    {
        AddDataRange(other.StartIndex, other.Data);
    }
    

    public bool IsIndexInRange(int index, int offset = 0)
    {
        return index >= StartIndex - offset && index <= EndIndex + offset;
    }
    private int MemoryToInternalIndex(int memIndex) => memIndex - StartIndex;
}