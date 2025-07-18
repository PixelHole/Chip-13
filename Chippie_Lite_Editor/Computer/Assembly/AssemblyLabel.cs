namespace Chippie_Lite_WPF.Computer.Assembly;

public class AssemblyLabel
{
    public string Name { get; private set; }
    public int Value { get; set; }


    public AssemblyLabel(string name, int value = 0)
    {
        Name = name;
        Value = value;
    }
}