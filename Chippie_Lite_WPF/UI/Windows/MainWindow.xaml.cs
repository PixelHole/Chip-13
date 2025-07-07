using System.Windows;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.Linkers;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Windows;


public partial class MainWindow : Window
{
    private MainWindowControl Control { get; set; }
    public AppMode Mode { get; private set; }
    
    
    public MainWindow()
    {
        WindowStyle = WindowStyle.None;
        InitializeComponent();
        Control = new MainWindowControl(this);
    }


    public void Run()
    {
        Control.RunScript(DevArea.GetInputString());
    }
    public void Halt()
    {
        Control.Halt();
    }

    public void ChangeDevAreaPage(int index)
    {
        RunDispatcher(() => DevArea.ChangePage(index));
    }
    public void SetMode(AppMode mode)
    {
        RunDispatcher(() =>
        {
            Mode = mode;
            DevArea.SetMode(Mode);
            switch (Mode)
            {
                case AppMode.Edit:
                    RunBtn.Visibility = Visibility.Visible;
                    StopBtn.Visibility = Visibility.Collapsed;
                    RestartBtn.Visibility = Visibility.Collapsed;
                    break;

                case AppMode.Run:
                    RunBtn.Visibility = Visibility.Collapsed;
                    StopBtn.Visibility = Visibility.Visible;
                    RestartBtn.Visibility = Visibility.Visible;
                    break;
            }
        });
    }

    public void ShowExceptionInCode(InvalidInstructionException exception)
    {
        RunDispatcher(() =>
        {
            DevArea.ChangePage(0);
            DevArea.ShowExceptionError(exception);
        });
    }
    public void ShowExceptionInCode(InstructionInterpretationException exception)
    {
        RunDispatcher(() =>
        {
            DevArea.ChangePage(0);
            DevArea.ShowExceptionError(exception);
        });
    }
    public void ShowError(string title, string msg)
    {
        RunDispatcher(() => DevArea.ShowError(title, msg));
    }
    
    private void StopBtn_OnClick(SquareButton sender)
    {
        Halt();
    }
    private void RunBtn_OnClick(SquareButton sender)
    {
        Run();
    }
    private void RestartBtn_OnClick(SquareButton sender)
    {
        Halt();
        Run();
    }
    
    private void ToolbarFileBtn_OnClick(SquareButton sender)
    {
        
    }
    private void ToolbarHelpBtn_OnClick(SquareButton sender)
    {
        
    }
    private void ToolbarLogoBtn_OnClick(SquareButton sender)
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

    private void RunDispatcher(Action action)
    {
        Dispatcher.Invoke(action);
    }
    private void DragBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            // TODO : window repositions upon resizing
        }
        DragMove();
    }
}