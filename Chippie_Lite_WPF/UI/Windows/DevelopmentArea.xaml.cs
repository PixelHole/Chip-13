using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Windows;

public partial class DevelopmentArea : UserControl
{
    public DevelopmentArea()
    {
        InitializeComponent();
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        PageSelect.OnPageChanged += OnPageTabChanged;
    }

    public void SetMode(AppMode mode)
    {
        switch (mode)
        {
            case AppMode.Run:
                CodeEditControl.SetEditable(false);
                break;

            case AppMode.Edit:
                CodeEditControl.SetEditable(true);
                break;
        }
    }
    
    public string GetInputString()
    {
        return CodeEditControl.InputText;
    }

    public void ShowExceptionError(InvalidInstructionException exception)
    {
        CodeEditControl.HighlightLine(exception.ActualLine);
        
        switch (exception)
        {
            case RegisterNotFoundException registerNotFoundException:
                ShowError($"(Line {exception.ActualLine + 1}) : Register not found",
                    $"the stated register with id ({registerNotFoundException.ProvidedId}) could not be found. " +
                    $"please make sure you have spelled the register id correctly");
                return;
            case InvalidHeaderException headerException:
                ShowError($"(Line {exception.ActualLine + 1}) : Invalid instruction header", 
                    $"invalid instruction header ({headerException.ProvidedHeader}). make sure the header is spelled correctly." +
                    $" if correct, it will take on a pink color");
                return;
            case NotEnoughArgumentsException notEnoughArgumentsException:
                ShowError($"Line {exception.ActualLine + 1}) : Not enough arguments",
                    $"instruction was not given enough arguments. it needs ({notEnoughArgumentsException.ExpectedCount})." +
                    $"you have given it ({notEnoughArgumentsException.ProvidedCount})");
                return;
            
            case InvalidArgumentException argumentException:
                ShowError($"Line {exception.ActualLine + 1}) : Invalid argument",
                    $"argument string ({argumentException.InputString}) does not match the type ({argumentException.ExpectedType})." +
                    $" make sure you have formatted your argument correctly and is the correct type." +
                    $" if so, the string will likely change color");
                return;
        }
    }
    public void ShowError(string title, string msg, double x = 0.5d, double y = 0.5d)
    {
        ErrorBox.Pop(x, y, title, msg);
    }

    public void SetSelectedTab(int index)
    {
        PageSelect.ChangeSelectedTab(index);
    }
    
    private void ChangePage(int index)
    {
        switch (index)
        {
            case 0 :
                CodeEditControl.Visibility = Visibility.Visible;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
            
            case 1 :
                CodeEditControl.Visibility = Visibility.Collapsed;
                MemoryEditControl.Visibility = Visibility.Visible;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
            
            case 2 :
                CodeEditControl.Visibility = Visibility.Collapsed;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Visible;
                break;
            
            default :
                CodeEditControl.Visibility = Visibility.Visible;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
        }
    }
    private void OnPageTabChanged(int index)
    {
        ChangePage(index);
    }
}