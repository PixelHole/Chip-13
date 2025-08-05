using System.Windows;
using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class WindowRibbon
{
    public WindowRibbon()
    {
        InitializeComponent();
    }
    private void MinimizeBtn_OnClick(SquareButton sender)
    {
        var window = Window.GetWindow(this);
        if (window == null) return;
        window.WindowState = WindowState.Minimized;
    }
    private void FullscreenBtn_OnClick(SquareButton sender)
    {
        var window = Window.GetWindow(this);
        if (window == null) return;

        window.WindowState = window.WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            WindowState.Normal => WindowState.Maximized,
            _ => window.WindowState
        };
    }
    private void CloseBtn_OnClick(SquareButton sender)
    {
        var window = Window.GetWindow(this);
        if (window == Application.Current.MainWindow) Application.Current.Shutdown();
        else window?.Close();
    }
}