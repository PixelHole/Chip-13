using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class MemoryViewLabel : UserControl
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(MemoryViewLabel),
        new PropertyMetadata(string.Empty)
        );
    
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    public MemoryViewLabel()
    {
        InitializeComponent();
    }
}