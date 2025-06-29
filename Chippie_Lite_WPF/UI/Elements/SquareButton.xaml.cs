using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class SquareButton : UserControl
{
    private bool MouseInside = false;
    private string _label;
    private ImageSource _icon;

    public delegate void ClickAction();
    public event ClickAction Click;
    
    
    public required string Label
    {
        get => _label;
        set
        {
            _label = value;
            TextDisplay.Content = _label;
        }
    }
    public ImageSource Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            ImageDisplay.Source = _icon;
        }
    }

    public Brush BackgroundHoverColor { get; set; } = new SolidColorBrush(Color.FromRgb(131, 118, 156));
    public Brush BackgroundDefaultColor { get; set; } = new SolidColorBrush(Color.FromRgb(29, 43, 83));
    public Brush BackgroundClickColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 119, 168));
    
    
    public SquareButton()
    {
        InitializeComponent();
        
    }
    
    
    private void SetColor(Brush color)
    {
        Body.Background = color;
    }
    
    private void Body_OnMouseEnter(object sender, MouseEventArgs e)
    {
        MouseInside = true;
        SetColor(BackgroundHoverColor);
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        MouseInside = false;
        SetColor(BackgroundDefaultColor);
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        SetColor(BackgroundClickColor);
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (MouseInside)
        {
            Click?.Invoke();
            // Console.Beep(500, 50);
        }
        SetColor(BackgroundHoverColor);
    }
}