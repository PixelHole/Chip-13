using System.Windows;
using System.Windows.Controls;

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
    private void MinimizeBtn_OnClick()
    {
        Minimize?.Invoke();
    }
    private void FullscreenBtn_OnClick()
    {
        Fullscreen?.Invoke();
    }
    private void CloseBtn_OnClick()
    {
        Close?.Invoke();
    }
}