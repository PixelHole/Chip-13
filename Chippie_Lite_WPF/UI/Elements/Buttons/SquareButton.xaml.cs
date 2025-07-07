using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class SquareButton : UserControl
{
    private bool mouseDown = false;
    private bool mouseInside = false;
    private string _label;
    private ImageSource _icon;
    
    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            TextDisplay.Text = _label;
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
    public Brush BackgroundDefaultColor { get; set; } = new SolidColorBrush(Colors.Transparent);
    public Brush BackgroundClickColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 119, 168));
    
    public delegate void ClickAction(SquareButton sender);
    public event ClickAction Click;
    
    
    public SquareButton()
    {
        InitializeComponent();
    }
    
    private void SetColor(Brush color)
    {
        Body.Background = color;
    }
    private void UpdateColor()
    {
        switch (mouseDown)
        {
            case true :
                SetColor(mouseInside ? BackgroundClickColor : BackgroundHoverColor);
                break;
            case false :
                SetColor(mouseInside ? BackgroundHoverColor : BackgroundDefaultColor);
                break;
        }
    }
    
    private void Body_OnMouseEnter(object sender, MouseEventArgs e)
    {
        mouseInside = true;
        UpdateColor();
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        mouseInside = false;
        mouseDown = false;
        UpdateColor();
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        mouseDown = true;
        UpdateColor();
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        mouseDown = false;
        UpdateColor();
        Click?.Invoke(this);
    }
}