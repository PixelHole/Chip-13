namespace Chippie_Lite_WPF.Computer.Assembly;

public static class AssemblyLabelDatabase
{
    private static readonly List<AssemblyLabel> Labels = new List<AssemblyLabel>();


    public static void ClearLabels()
    {
        Labels.Clear();
    }
    public static bool AddLabel(AssemblyLabel label)
    {
        if (GetLabel(label.Name) != null) return false;
        Labels.Add(label);
        return true;
    }
    public static AssemblyLabel? GetLabel(string name)
    {
        return FindLabel(label => label.Name == name);
    }
    public static AssemblyLabel? FindLabel(Predicate<AssemblyLabel> predicate) => Labels.Find(predicate);
}