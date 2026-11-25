using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config;

public partial class TemplateListItem : UserControl
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(TemplateListItem),
        new PropertyMetadata(false)
    );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    private InstructionTemplate? template;

    public InstructionSetEdit View;
    public new InstructionTemplate? Template
    {
        get => template;
        set
        {
            template = value;
            LoadTemplateData();
        }
    }
    
    public TemplateListItem(InstructionSetEdit view)
    {
        InitializeComponent();
        View = view;
    }

    private void LoadTemplateData()
    {
        WPFUtility.SetContent(NameLabel, Template?.Name ?? "Name");
        WPFUtility.SetContent(HeaderLabel, Template?.Header ?? "Header");
    }
    
    private void EditBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (Template != null) View.EditRequest(this, Template);
    }
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (Template != null) View.DuplicateRequest(this, Template);
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (Template != null) View.DeleteRequest(this, Template);
    }
    
    private void InstructionSetListItem_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void InstructionSetListItem_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;
    }
    private void InstructionSetListItem_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
    private void InstructionSetListItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (Template != null) View.EditRequest(this, Template);
    }
}