using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class SquareButton : UserControl
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(SquareButton),
        new PropertyMetadata(false)
    );
    public static readonly DependencyProperty DefaultColorProperty = DependencyProperty.Register(
        nameof(BackgroundDefaultColor),
        typeof(Brush),
        typeof(SquareButton),
        new PropertyMetadata(new SolidColorBrush(Colors.Transparent))
    );
    public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(
        nameof(BackgroundHoverColor),
        typeof(Brush),
        typeof(SquareButton),
        new PropertyMetadata(new SolidColorBrush(Color.FromRgb(131, 118, 156)))
        );
    public static readonly DependencyProperty ClickColorProperty = DependencyProperty.Register(
        nameof(BackgroundClickColor),
        typeof(Brush),
        typeof(SquareButton),
        new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 119, 168)))
    );
    
    private string _label = null!;
    private ImageSource _icon = null!;
    
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

    public Brush BackgroundHoverColor 
    { 
        get => (Brush)GetValue(DefaultColorProperty);
        set => SetValue(DefaultColorProperty, value);
    }
    public Brush BackgroundDefaultColor 
    { 
        get => (Brush)GetValue(HoverColorProperty);
        set => SetValue(HoverColorProperty, value);
    }
    public Brush BackgroundClickColor 
    { 
        get => (Brush)GetValue(ClickColorProperty);
        set => SetValue(ClickColorProperty, value);
    }

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    public delegate void ClickAction(SquareButton sender, MouseButtonEventArgs e);
    public event ClickAction? Click;
    
    
    public SquareButton()
    {
        InitializeComponent();
    }
    
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        Click?.Invoke(this, e);
        IsPressed = false;
        e.Handled = true;
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
}