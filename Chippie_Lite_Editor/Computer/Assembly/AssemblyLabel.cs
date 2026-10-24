namespace Chippie_Lite_WPF.Computer.Assembly;

public class AssemblyLabel
{
    public string Name { get; private set; }
    public long Value { get; set; }


    public AssemblyLabel(string name, long value = 0)
    {
        Name = name;
        Value = value;
    }
}