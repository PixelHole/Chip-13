using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ColorPickerWindow : Window
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(ColorPickerWindow),
        new PropertyMetadata("Pick Color")
        );
    
    public delegate void ConfirmAction(object sender, Color color);
    public event ConfirmAction? OnConfirm;

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    
    public ColorPickerWindow(Color color)
    {
        InitializeComponent();
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        PickerElement.SelectedColor = color;
    }
    
    private void OkBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        OnConfirm?.Invoke(this, PickerElement.SelectedColor);
        Close();
    }
    private void CancelBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Close();
    }
}