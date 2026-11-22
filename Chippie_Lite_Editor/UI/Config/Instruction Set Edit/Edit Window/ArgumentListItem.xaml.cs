using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config.Edit_Window;

public partial class ArgumentListItem : UserControl
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(ArgumentListItem),
        new PropertyMetadata(false)
    );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    public int SelectedIndex
    {
        get
        {
            int index = -1;
            WPFUtility.DoUiAction(TypeBox, () => index = TypeBox.SelectedIndex);
            return index;
        }
        set => WPFUtility.DoUiAction(TypeBox, () => TypeBox.SelectedIndex = value);
    }
    
    public delegate void TypeSelectAction(ArgumentListItem sender, InstructionArgumentType type);
    public delegate void DeleteRequestAction(ArgumentListItem sender, InstructionArgumentType type);
    public delegate void DuplicateRequestAction(ArgumentListItem sender, InstructionArgumentType type);
    public event TypeSelectAction? OnTypeSelected;
    public event DeleteRequestAction? OnDeleteRequested;
    public event DuplicateRequestAction? OnDuplicateRequested;
    
    public ArgumentListItem()
    {
        InitializeComponent();
    }
    public ArgumentListItem(int selectionIndex) : this()
    {
        InitializeComponent();
        SelectedIndex = selectionIndex;
    }
    
    private void Selector_OnSelected(object sender, RoutedEventArgs e)
    {
        InstructionArgumentType selectedType = (InstructionArgumentType)TypeBox.SelectedIndex;
        OnTypeSelected?.Invoke(this, selectedType);
    }
    
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        OnDuplicateRequested?.Invoke(this, (InstructionArgumentType)SelectedIndex);
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        OnDeleteRequested?.Invoke(this, (InstructionArgumentType)SelectedIndex);
    }
    
    private void ArgumentListItem_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void ArgumentListItem_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;
    }
    private void ArgumentListItem_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
}