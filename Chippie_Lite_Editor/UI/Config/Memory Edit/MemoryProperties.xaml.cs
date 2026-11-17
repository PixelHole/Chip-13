using System.Windows.Controls;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Config;

public partial class MemoryProperties : UserControl
{
    private Memory? assignedMemory;

    public Memory? AssignedMemory
    {
        get => assignedMemory;
        set
        {
            assignedMemory = value;
            LoadMemoryData();
        }
    }
    
    
    public MemoryProperties()
    {
        InitializeComponent();
    }

    private void LoadMemoryData()
    {
        if (AssignedMemory == null) return;
        NameBox.Text = AssignedMemory.Name;
        SizeBox.Text = AssignedMemory.Size.ToString();
        WordLengthBox.Text = AssignedMemory.LengthLimit.ToString();
        PermanentToggle.Selected = AssignedMemory.Permanent;
        ReadOnlyToggle.Selected = AssignedMemory.ReadOnly;
        MemoryView.AssignedMemory = AssignedMemory;
    }
    
    public bool NameCondition(string s)
    {
        var mems = ChippieComponentManager.GetComponentsOfType<Memory>();
        return s.All(char.IsAscii) && mems.All(m => m.Name != s);
    }
    public bool SizeCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out _);
    }
    public bool WordLengthCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out var num) && num is >= 1 and <= 64;
    }
}