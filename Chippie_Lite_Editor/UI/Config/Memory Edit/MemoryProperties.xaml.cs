using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config;

public partial class MemoryProperties
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

    private bool IsUpdating = false;
    
    public delegate void CancelAction(MemoryProperties sender, Memory memory);
    public delegate void ConfirmAction(MemoryProperties sender, Memory memory);
    public event CancelAction? OnCancelRequest;
    public event ConfirmAction? OnConfirmRequest;
    
    public MemoryProperties()
    {
        InitializeComponent();
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        MemoryView.OnMemoryModuleChanged += MemoryViewOnOnMemoryModuleChanged;
    }
    private void MemoryViewOnOnMemoryModuleChanged(MemoryView sender, Memory memory)
    {
        IsUpdating = true;
        AssignedMemory = memory;
        IsUpdating = false;
    }

    private void LoadMemoryData()
    {
        if (AssignedMemory == null) return;
        NameBox.Text = AssignedMemory.Name;
        SizeBox.Text = AssignedMemory.Size.ToString();
        WordLengthBox.Text = AssignedMemory.LengthLimit.ToString();
        PermanentToggle.Selected = AssignedMemory.Permanent;
        ReadOnlyToggle.Selected = AssignedMemory.ReadOnly;
        if (IsUpdating) return;
        MemoryView.AssignedMemory = AssignedMemory;
    }
    
    public bool NameCondition(string s)
    {
        return s.All(char.IsAscii);
    }
    public bool SizeCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out _);
    }
    public bool WordLengthCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out var num) && num is >= 1 and <= 64;
    }
    
    private void NameBox_OnTextConfirmed(string text)
    {
        if (AssignedMemory != null) AssignedMemory.Name = text;
    }
    private void SizeBox_OnTextConfirmed(string text)
    {
        if (AssignedMemory != null) AssignedMemory.Size = (int)NumberUtility.ParseNumber(text);
    }
    private void WordLengthBox_OnTextConfirmed(string text)
    {
        if (AssignedMemory != null) AssignedMemory.LengthLimit = (byte)NumberUtility.ParseNumber(text);
    }
    
    private void ConfirmBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if(AssignedMemory != null) OnConfirmRequest?.Invoke(this, AssignedMemory);
    }
    private void CancelBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if(AssignedMemory != null) OnCancelRequest?.Invoke(this, AssignedMemory);
    }
}