using System.Windows;
using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Elements;

public class TitledFrame : ContentControl
{
    static TitledFrame()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(TitledFrame),
            new FrameworkPropertyMetadata(typeof(TitledFrame)));
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(TitledFrame),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty InnerContentProperty =
        DependencyProperty.Register(
            nameof(TopContent), 
            typeof(object), 
            typeof(TitledFrame), 
            new PropertyMetadata(null));
    
    
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public object TopContent
    {
        get => GetValue(InnerContentProperty);
        set => SetValue(InnerContentProperty, value);
    }
}