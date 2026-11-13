using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ToggleButton : UserControl
{
    public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
        nameof(Selected),
        typeof(bool),
        typeof(ToggleButton),
        new PropertyMetadata(false)
        );
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(ToggleButton),
        new PropertyMetadata(false)
    );
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(ImageSource),
        typeof(ToggleButton),
        new PropertyMetadata(null!)
    );
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label),
        typeof(string),
        typeof(ToggleButton),
        new PropertyMetadata(null!)
    );
    
    public static readonly DependencyProperty UnselectedDefaultProperty = DependencyProperty.Register(
        nameof(UnselectedDefaultColor),
        typeof(Brush),
        typeof(ToggleButton),
        new PropertyMetadata(new SolidColorBrush(Colors.Transparent))
    );
    public static readonly DependencyProperty UnselectedHoverProperty = DependencyProperty.Register(
        nameof(UnselectedHoverProperty),
        typeof(Brush),
        typeof(ToggleButton),
        new PropertyMetadata(new SolidColorBrush(Color.FromRgb(131, 118, 156)))
    );
    public static readonly DependencyProperty UnselectedClickProperty = DependencyProperty.Register(
        nameof(UnselectedClickColor),
        typeof(Brush),
        typeof(ToggleButton),
        new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 119, 168)))
    );
    public static readonly DependencyProperty SelectedDefaultProperty = DependencyProperty.Register(
        nameof(SelectedDefaultColor),
        typeof(Brush),
        typeof(ToggleButton),
        new PropertyMetadata(new SolidColorBrush(Color.FromRgb(131, 118, 156)))
    );
    public static readonly DependencyProperty SelectedHoverProperty = DependencyProperty.Register(
        nameof(SelectedHoverColor),
        typeof(Brush),
        typeof(ToggleButton),
        new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 119, 168)))
    );
    public static readonly DependencyProperty SelectedClickProperty = DependencyProperty.Register(
        nameof(SelectedClickColor),
        typeof(Brush),
        typeof(ToggleButton),
        new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 204, 170)))
    );
    
    public bool Selected
    {
        get => (bool)GetValue(SelectedProperty);
        set => SetValue(SelectedProperty, value);
    }
    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Brush UnselectedDefaultColor
    {
        get => (Brush)GetValue(UnselectedDefaultProperty);
        set => SetValue(UnselectedDefaultProperty, value);
    }
    public Brush UnselectedHoverColor
    {
        get => (Brush)GetValue(UnselectedHoverProperty);
        set => SetValue(UnselectedHoverProperty, value);
    }
    public Brush UnselectedClickColor
    {
        get => (Brush)GetValue(UnselectedClickProperty);
        set => SetValue(UnselectedClickProperty, value);
    }
    public Brush SelectedDefaultColor
    {
        get => (Brush)GetValue(SelectedDefaultProperty);
        set => SetValue(SelectedDefaultProperty, value);
    }
    public Brush SelectedHoverColor
    {
        get => (Brush)GetValue(SelectedHoverProperty);
        set => SetValue(SelectedHoverProperty, value);
    }
    public Brush SelectedClickColor
    {
        get => (Brush)GetValue(SelectedClickProperty);
        set => SetValue(SelectedClickProperty, value);
    }
    
    public delegate void SelectAction(ToggleButton sender, bool selected);
    public event SelectAction OnSelected = null!;
    

    public ToggleButton()
    {
        InitializeComponent();
    }
    
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;
        if (IsMouseOver)
        {
            Selected = !Selected;
            OnSelected?.Invoke(this, Selected);
        }
        e.Handled = true;
    }
}