using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class WindowRibbon : UserControl
{ 
    public delegate void MinimizeAction();
    public event MinimizeAction Minimize;
    public delegate void FullscreenAction();
    public event FullscreenAction Fullscreen;
    public delegate void CloseAction();
    public event CloseAction Close;
    
    public WindowRibbon()
    {
        InitializeComponent();
    }
    private void MinimizeBtn_OnClick(SquareButton sender)
    {
        Minimize?.Invoke();
    }
    private void FullscreenBtn_OnClick(SquareButton sender)
    {
        Fullscreen?.Invoke();
    }
    private void CloseBtn_OnClick(SquareButton sender)
    {
        Close?.Invoke();
    }
}