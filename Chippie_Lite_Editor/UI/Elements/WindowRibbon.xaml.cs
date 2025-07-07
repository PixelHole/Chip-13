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
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }
    private void FullscreenBtn_OnClick(SquareButton sender)
    {
        if (Application.Current.MainWindow == null) return;

        Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            WindowState.Normal => WindowState.Maximized,
            _ => Application.Current.MainWindow.WindowState
        };
    }
    private void CloseBtn_OnClick(SquareButton sender)
    {
        Application.Current.Shutdown();
    }
}