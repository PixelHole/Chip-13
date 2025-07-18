using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class MemoryViewLabel : UserControl
{
    public string Text
    {
        get => DisplayText.Text;
        set => DisplayText.Text = value;
    }

    public double TextSize
    {
        get => DisplayText.FontSize;
        set => DisplayText.FontSize = value;
    }


    public MemoryViewLabel()
    {
        InitializeComponent();
    }
}