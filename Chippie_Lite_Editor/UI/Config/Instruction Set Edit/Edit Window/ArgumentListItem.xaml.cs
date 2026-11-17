using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config.Edit_Window;

public partial class ArgumentListItem : UserControl
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(ColorListEditItem),
        new PropertyMetadata(false)
    );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    
    public ArgumentListItem()
    {
        InitializeComponent();
    }
    
    private void Selector_OnSelected(object sender, RoutedEventArgs e)
    {
        
    }
    
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        
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