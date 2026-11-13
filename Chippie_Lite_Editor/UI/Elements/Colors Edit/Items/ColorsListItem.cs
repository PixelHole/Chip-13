using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public abstract class ColorsListItem : UserControl
{
    private int index = -1;
    private Brush assignedBrush = new SolidColorBrush(Colors.Black);
    public Brush AssignedBrush
    {
        get => assignedBrush;
        set
        {
            if (assignedBrush == value) return;
            Brush old = assignedBrush.Clone();
            assignedBrush = value;
            WPFUtility.DoUiAction(this, () => ColorUpdated(old, assignedBrush));
        }
    }
    public int Index
    {
        get => index;
        set
        {
            if (index == value) return;
            index = value;
            WPFUtility.DoUiAction(this, () => IndexUpdated(index));
        }
    }

    public Color AssignedColor => (AssignedBrush as SolidColorBrush)?.Color ?? Colors.Black;
    
    protected abstract void ColorUpdated(Brush old, Brush newBrush);
    protected abstract void IndexUpdated(int index);
}