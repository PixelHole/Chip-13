namespace Chippie_Lite_WPF.Computer.Utility;

public static class IndexUtility
{
    public static int WrapIndex(int index, int count)
    {
        if (count == 0) return 0;
        if (index >= count) return index % count;
        if (index < 0) return (count - 1) -index % count;
        return index;
    }
}