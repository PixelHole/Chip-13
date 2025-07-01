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

    public bool Sticky { get; set; }
    public bool Selected { get; private set; }
    
    public delegate void ClickAction(SquareButton sender);
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
    public Brush BackgroundDefaultColor { get; set; } = new SolidColorBrush(Colors.Transparent);
    public Brush BackgroundClickColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 119, 168));
    public Brush BackgroundHighLightColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 241, 232));
    
    
    public SquareButton()
    {
        InitializeComponent();
        // SetColor(BackgroundDefaultColor);
    }


    public void SetSelected(bool selected)
    {
        Selected = selected;
        switch (MouseInside)
        {
            case true when Selected:
                SetColor(BackgroundHighLightColor);
                break;
            
            case true when !Selected:
                SetColor(BackgroundClickColor);
                break;
            
            case false when Selected:
                SetColor(BackgroundHoverColor);
                break;
            
            case false when !Selected:
                SetColor(BackgroundDefaultColor);
                break;
        }
    }
    private void SetColor(Brush color)
    {
        Body.Background = color;
    }
    
    private void Body_OnMouseEnter(object sender, MouseEventArgs e)
    {
        MouseInside = true;
        switch (Sticky)
        {
            case true when !Selected:
                SetColor(BackgroundHoverColor);
                break;
            case true when Selected:
                SetColor(BackgroundClickColor);
                break;
            case false :
                SetColor(BackgroundHoverColor);
                break;
        }
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        MouseInside = false;
        switch (Sticky)
        {
            case true when !Selected:
                SetColor(BackgroundDefaultColor);
                break;
            case true when Selected:
                SetColor(BackgroundHoverColor);
                break;
            case false :
                SetColor(BackgroundDefaultColor);
                break;
        }
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        SetColor(BackgroundClickColor);
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (MouseInside)
        {
            Click?.Invoke(this);
            Selected = !Selected;
            // Console.Beep(500, 50);
        }

        switch (Sticky)
        {
            case false :
                SetColor(BackgroundHoverColor);
                break;
            case true when Selected :
                SetColor(BackgroundClickColor);
                break;
            case true when !Selected :
                SetColor(BackgroundHoverColor);
                break;
        }
    }
}