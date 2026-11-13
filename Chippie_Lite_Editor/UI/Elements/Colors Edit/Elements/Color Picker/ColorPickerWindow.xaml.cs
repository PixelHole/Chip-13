using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ColorPickerWindow : Window
{
    public delegate void ConfirmAction(object sender, Color color);
    public event ConfirmAction? OnConfirm;
    
    
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