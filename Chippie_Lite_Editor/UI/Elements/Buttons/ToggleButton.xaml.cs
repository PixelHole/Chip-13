using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ToggleButton : UserControl
{
    private bool mouseInside = false;
    private bool mouseDown = false;
    private bool _selected = false;
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            UpdateColor();
        } 
    }
    
    public delegate void SelectAction(ToggleButton sender, bool selected);
    public event SelectAction OnSelected = null!;
    
    private string _label = null!;
    private ImageSource _icon = null!;
    
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

    public Brush UnselectedDefaultColor { get; set; } = new SolidColorBrush(Colors.Transparent);
    public Brush UnselectedHoverColor { get; set; } = new SolidColorBrush(Color.FromRgb(131, 118, 156));
    public Brush UnselectedClickColor { get; set; }  = new SolidColorBrush(Color.FromRgb(255, 119, 168));
    public Brush SelectedDefaultColor { get; set; } = new SolidColorBrush(Color.FromRgb(131, 118, 156));
    public Brush SelectedHoverColor { get; set; }  = new SolidColorBrush(Color.FromRgb(255, 119, 168));
    public Brush SelectedClickColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 204, 170));
    
    

    public ToggleButton()
    {
        InitializeComponent();
    }
    
    private void UpdateColor()
    {
        switch (Selected)
        {
            case true :
                switch (mouseDown)
                {
                    case true :
                        SetColor(SelectedClickColor);
                        break;
                    
                    case false :
                        SetColor(mouseInside ? SelectedHoverColor : SelectedDefaultColor);
                        break;
                }
                break;
            case false :
                switch (mouseDown)
                {
                    case true :
                        SetColor(UnselectedClickColor);
                        break;
                    
                    case false :
                        SetColor(mouseInside ? UnselectedHoverColor : UnselectedDefaultColor);
                        break;
                }
                break;
        }
    }
    private void SetColor(Brush color)
    {
        Body.Background = color;
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
        if (mouseInside)
        {
            Selected = !Selected;
            OnSelected?.Invoke(this, Selected);
        }
        UpdateColor();
        e.Handled = true;
    }
}