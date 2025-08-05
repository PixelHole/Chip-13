using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class PageTab : UserControl
{
    public Brush BackgroundHoverColor { get; set; } = new SolidColorBrush(Color.FromRgb(131, 118, 156));
    public Brush BackgroundDefaultColor { get; set; } = new SolidColorBrush(Colors.Transparent);
    public Brush BackgroundClickColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 119, 168));
    public Brush BackgroundHighLightColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 241, 232));
    
    public ImageSource IconSource
    {
        set => Icon.Source = value;
    }

    public string Text
    {
        set => TextDisplay.Content = value;
    }

    private bool _selected;

    public bool Selected
    {
        get
        {
            return _selected;
        } 
        set
        {
            _selected = value;
            UpdateColor();
        }
    }

    private bool MouseInside { get; set; }

    public delegate void SelectedAction(PageTab sender);
    public event SelectedAction? OnSelected;
    
    
    public PageTab()
    {
        InitializeComponent();
    }

    private void UpdateColor()
    {
        if (Selected)
        {
            SetColor(MouseInside? BackgroundClickColor : BackgroundHoverColor);
        }
        else
        {
            SetColor(MouseInside? BackgroundHoverColor : BackgroundDefaultColor);
        }
    }
    
    private void SetColor(Brush color)
    {
        Body.Background = color;
    }
    
    private void Body_OnMouseEnter(object sender, MouseEventArgs e)
    {
        MouseInside = true;
        UpdateColor();
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        MouseInside = false;
        UpdateColor();
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        UpdateColor();
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        Selected = true;
        OnSelected?.Invoke(this);
    }
}