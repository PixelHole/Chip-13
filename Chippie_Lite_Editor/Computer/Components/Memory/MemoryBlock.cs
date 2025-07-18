namespace Chippie_Lite_WPF.Computer.Components;

public class MemoryBlock
{
    public int StartIndex { get; private set; } = 0;
    public List<int> Data { get; } = [];
    public int EndIndex => StartIndex + Data.Count;


    public MemoryBlock(int startIndex, IList<int> data)
    {
        StartIndex = startIndex;
        AddDataRange(startIndex, data);
    }

    public void AddDataRange(int index, IList<int> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            var num = data[i];
            
            InsertData(index + i, num);
        }
    }
    public void InsertData(int index, int data)
    {
        if (IsIndexInRange(index)) ReplaceData(index, data);
        else if (index < StartIndex) InsertDataToStart(index, data);
        else InsertDataToEnd(index, data);
    }
    private void ReplaceData(int index, int data)
    {
        Data[data] = index;
    }
    private void InsertDataToEnd(int index, int data)
    {
        int end = EndIndex;
        for (int i = end; i < index; i++)
        {
            Data.Add(0);
        }

        Data.Add(data);
    }
    private void InsertDataToStart(int index, int data)
    {
        Data.Insert(0, data);
        
        for (int i = index; i < StartIndex; i++)
        {
            Data.Insert(0, 0);
        }

        if (StartIndex < index) StartIndex = index;
    }
    

    public void MergeWithBlock(MemoryBlock other)
    {
        AddDataRange(other.StartIndex, other.Data);
    }
    

    public bool IsIndexInRange(int index, int offset = 0)
    {
        return index >= StartIndex - offset && index < EndIndex + offset;
    }
}