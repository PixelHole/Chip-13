using System.Windows;
using Application = System.Windows.Application;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class WindowRibbon
{
    private byte visibleButtons = 7;

    // Don work :(
    // public byte VisibleButtons
    // {
    //     get => visibleButtons;
    //     set
    //     {
    //         if (visibleButtons == value) return;
    //         visibleButtons = value;
    //         UpdateButtons();
    //     }
    // }
    
    public WindowRibbon()
    {
        InitializeComponent();
    }

    // private void UpdateButtons()
    // {
    //     Visibility close = (VisibleButtons & 1) == 1 ? Visibility.Visible : Visibility.Collapsed, 
    //         maximize = (VisibleButtons & 2) == 2 ? Visibility.Visible : Visibility.Collapsed, 
    //         minimize = (VisibleButtons & 4) == 4 ? Visibility.Visible : Visibility.Collapsed;
    //
    //     WPFUtility.SetVisibility(CloseBtn, close);
    //     WPFUtility.SetVisibility(FullscreenBtn, maximize);
    //     WPFUtility.SetVisibility(MinimizeBtn, minimize);
    // }
    
    private void MinimizeBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window == null) return;
        window.WindowState = WindowState.Minimized;
    }
    private void FullscreenBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
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
    private void CloseBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window == Application.Current.MainWindow) Application.Current.Shutdown();
        else window?.Close();
    }
}