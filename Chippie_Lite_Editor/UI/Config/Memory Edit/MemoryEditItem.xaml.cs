using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config;

public partial class MemoryEditItem : UserControl
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(MemoryEditItem),
        new PropertyMetadata(false)
    );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    private Memory? assignedMemory = null!;
    
    public Memory? AssignedMemory
    {
        get => assignedMemory;
        set
        {
            if (assignedMemory == value) return;
            assignedMemory = value;
            LoadMemoryData();
        }
    }

    
    public delegate void DuplicateAction(MemoryEditItem sender, Memory memory);
    public delegate void EditAction(MemoryEditItem sender, Memory memory);
    public delegate void DeleteAction(MemoryEditItem sender, Memory memory);
    public event DuplicateAction? OnDuplicateRequest;
    public event EditAction? OnEditRequest;
    public event DeleteAction? OnDeleteRequest;
    
    public MemoryEditItem()
    {
        InitializeComponent();
    }

    public void LoadMemoryData()
    {
        if (AssignedMemory == null) return;
        WPFUtility.SetContent(NameLabel, AssignedMemory.Name);
        WPFUtility.SetContent(SizeLabel, AssignedMemory.Size + " Cells");
        WPFUtility.SetContent(WordLengthLabel, AssignedMemory.LengthLimit + " bits");
        WPFUtility.SetContent(ByteSizeLabel, AssignedMemory.GetSizeString());
        RefreshAttributesList();
    }
    private void RefreshAttributesList()
    {
        AttributesPanel.Children.Clear();
        if (AssignedMemory == null) return;
        if (AssignedMemory.Permanent) AddAttributeToList("Permanent");
        if (AssignedMemory.ReadOnly) AddAttributeToList("Read Only");
    }
    private void AddAttributeToList(string attribute)
    {
        MemoryAttribute attributeItem = new MemoryAttribute(attribute)
        {
            Margin = new Thickness(0, 0, 0, 4)
        };
        AttributesPanel.Children.Add(attributeItem);
    }

    private void EditRequest()
    {
        if (AssignedMemory != null) OnEditRequest?.Invoke(this, AssignedMemory);
    }
    private void DuplicateRequest()
    {
        if (AssignedMemory != null) OnDuplicateRequest?.Invoke(this, AssignedMemory);
    }
    private void DeleteRequest()
    {
        if (AssignedMemory != null) OnDeleteRequest?.Invoke(this, AssignedMemory);
    }
    
    private void EditBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        EditRequest();
    }
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        DuplicateRequest();
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        DeleteRequest();
    }
    
    private void MemoryEditItem_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void MemoryEditItem_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;
    }
    private void MemoryEditItem_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
    private void MemoryEditItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        EditRequest();
    }
}