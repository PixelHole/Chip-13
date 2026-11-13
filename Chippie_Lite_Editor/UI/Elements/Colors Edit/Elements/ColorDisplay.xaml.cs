using System.Windows.Controls;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ColorDisplay : Border
{
    private Brush color;
    
    public Brush Color
    {
        get => color;
        set
        {
            if (color == value) return;
            color = value;
            UpdateColor();
        }
    }
    
    public ColorDisplay()
    {
        InitializeComponent();
        Color = Background;
    }

    private void UpdateColor()
    {
        WPFUtility.DoUiAction(this, () => Background = Color);
        WPFUtility.SetContent(HexCodeLabel, GetColorHex());
    }
    private string GetColorHex()
    {
        return Color is not SolidColorBrush solidColorBrush
            ? "Custom"
            : $"#{solidColorBrush.Color.R:X2}{solidColorBrush.Color.G:X2}{solidColorBrush.Color.B:X2}";
    }
}