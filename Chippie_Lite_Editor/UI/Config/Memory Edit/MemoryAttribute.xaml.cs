using System.Windows;
using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Config;

public partial class MemoryAttribute
{
    public static readonly DependencyProperty AttributeProperty = DependencyProperty.Register(
        nameof(Attribute),
        typeof(string),
        typeof(MemoryAttribute),
        new PropertyMetadata("Property")
        );
    public string Attribute
    {
        get => (string)GetValue(AttributeProperty);
        set => SetValue(AttributeProperty, value);
    }
    
    public MemoryAttribute()
    {
        InitializeComponent();
    }
    public MemoryAttribute(string attribute) : this()
    {
        Attribute = attribute;
    }
}