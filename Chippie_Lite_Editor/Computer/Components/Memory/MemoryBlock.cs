namespace Chippie_Lite_WPF.Computer.Components;

public class MemoryBlock
{
    public int StartIndex { get; private set; } = 0;
    public List<int> Data { get; } = [];

    public int EndIndex
    {
        get
        {
            if (Data.Count < 1) return StartIndex;
            return StartIndex + Data.Count - 1;
        }
    }

    public int FilledCells { get; private set; } = 0;


    public MemoryBlock(int startIndex, IList<int> data)
    {
        StartIndex = startIndex;
        AddDataRange(startIndex, data);
    }

    public int Read(int memIndex)
    {
        return Data[MemoryToInternalIndex(memIndex)];
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
        if (IsIndexInRange(memIndex) && Data.Count > 0) ReplaceData(memIndex, data);
        else if (memIndex < StartIndex) InsertDataToStart(memIndex, data);
        else InsertDataToEnd(memIndex, data);
    }
    private void ReplaceData(int memIndex, int data)
    {
        Data[MemoryToInternalIndex(memIndex)] = data;
        if (data == 0) FilledCells--;
        else FilledCells++;
    }
    private void InsertDataToEnd(int memIndex, int data)
    {
        int end = EndIndex;
        for (int i = end; i < memIndex - 1; i++)
        {
            Data.Add(0);
        }

        Data.Add(data);

        if (data != 0) FilledCells++;
    }
    private void InsertDataToStart(int memIndex, int data)
    {
        Data.Insert(0, data);
        
        for (int i = memIndex; i < StartIndex; i++)
        {
            Data.Insert(0, 0);
        }

        if (StartIndex < memIndex) StartIndex = memIndex;

        if (data != 0) FilledCells++;
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