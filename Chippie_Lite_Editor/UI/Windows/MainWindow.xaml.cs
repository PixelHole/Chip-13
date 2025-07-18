using System.Windows;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.Controls;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Windows;


public partial class MainWindow
{
    private MainWindowControl Control { get; }
    private AppMode Mode { get; set; }
    
    
    public MainWindow()
    {
        WindowStyle = WindowStyle.None;
        InitializeComponent();
        Control = new MainWindowControl(this);
    }


    private void Run()
    {
        Control.RunScript(DevArea.GetInputCode());
    }
    private void Halt()
    {
        Control.Halt();
    }
    private void Restart()
    {
        Control.Restart();
    }
    
    public void ChangeDevAreaPage(int index)
    {
        RunDispatcher(() => DevArea.ChangePage(index));
        RunDispatcher(() => UpdateTabButtons(index));
    }
    private void UpdateTabButtons(int index)
    {
        ToggleButton[] buttons = [CodeWindowBtn, MemoryWindowBtn, RunWindowBtn/*, ConsoleWindowBtn*/];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Selected = i == index;
        }
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
                    RunToEndBtn.Visibility = Visibility.Collapsed;
                    SingleStepBtn.Visibility = Visibility.Collapsed;
                    break;

                case AppMode.Run:
                    RunBtn.Visibility = Visibility.Collapsed;
                    StopBtn.Visibility = Visibility.Visible;
                    RestartBtn.Visibility = Visibility.Visible;
                    RunToEndBtn.Visibility = Visibility.Visible;
                    SingleStepBtn.Visibility = Visibility.Visible;
                    break;
            }
        });
    }

    public void RunToEndButtonVisible(bool visible)
    {
        RunDispatcher(() => RunToEndBtn.Visibility = visible ? Visibility.Visible : Visibility.Collapsed);
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
        Restart();
    }
    private void SingleStepBtn_OnClick(SquareButton sender)
    {
        Control.NextStep();
    }
    private void RunToEndBtn_OnClick(SquareButton sender)
    {
        Control.RunToEnd();
    }
    
    private void ToolbarLogoBtn_OnClick(SquareButton sender)
    {
        
    }
    private void ToolbarNewBtn_OnClick(SquareButton sender)
    {
        Control.RequestNewInstance();
    }
    private void ToolbarSaveBtn_OnClick(SquareButton sender)
    {
        Control.RequestSave();
    }
    private void ToolbarSaveAsBtn_OnClick(SquareButton sender)
    {
        Control.RequestSaveAs();
    }
    private void ToolbarOpenBtn_OnClick(SquareButton sender)
    {
        Control.RequestLoad();
    }
    private void ToolbarHelpBtn_OnClick(SquareButton sender)
    {
        
    }
    
    private void CodeWindowBtn_OnOnSelected(ToggleButton sender, bool selected)
    {
        ChangeDevAreaPage(0);
        UpdateTabButtons(0);
    }
    private void MemoryWindowBtn_OnOnSelected(ToggleButton sender, bool selected)
    {
        ChangeDevAreaPage(1);
        UpdateTabButtons(1);
    }
    private void RunWindowBtn_OnOnSelected(ToggleButton sender, bool selected)
    {
        ChangeDevAreaPage(2);
        UpdateTabButtons(2);
    }
    private void ConsoleWindowBtn_OnOnSelected(ToggleButton sender, bool selected)
    {
        UpdateTabButtons(3);
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