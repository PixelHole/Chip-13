using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chippie_Lite_WPF;


public partial class MainWindow : Window
{
    public int Mode { get; private set; }
    
    
    public MainWindow()
    {
        WindowStyle = WindowStyle.None;
        InitializeComponent();
    }
    private void Toolbar_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        // TODO : figure this out later
        // DragMove();
    }
    
    
    private void ToolbarFileBtn_OnClick()
    {
        
    }
    private void ToolbarRunBtn_OnClick()
    {
        
    }
    private void ToolbarHelpBtn_OnClick()
    {
        
    }
    private void ToolbarLogoBtn_OnClick()
    {
        
    }
    
    private void WindowRibbon_OnMinimize()
    {
        WindowState = WindowState.Minimized;
    }
    private void WindowRibbon_OnFullscreen()
    {
        WindowState = WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            WindowState.Normal => WindowState.Maximized,
            _ => WindowState
        };
    }
    private void WindowRibbon_OnClose()
    {
        Application.Current.Shutdown();
    }
}