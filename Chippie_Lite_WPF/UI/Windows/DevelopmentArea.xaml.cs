using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
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
                RunModeProcedure();
                break;

            case AppMode.Edit:
                EditModeProcedure();
                break;
        }
    }

    private void EditModeProcedure()
    {
        CodeEditControl.SetEditable(true);
    }
    private void RunModeProcedure()
    {
        CodeEditControl.SetEditable(false);
    }
    
    
    // code edit window
    public string GetInputString()
    {
        return CodeEditControl.InputText;
    }
    

    // page select
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
    
    // Error box
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
    public void ShowExceptionError(InstructionInterpretationException exception)
    {
        CodeEditControl.HighlightLine(exception.Instruction.ScriptLine);
        
        switch (exception)
        {
            case ActionIndexOutOfRangeException actionIndexOutOfRangeException :
                ShowError($"(Line {exception.Instruction.ScriptLine}) " +
                          $"Action Index out of range",
                    $"the index ({actionIndexOutOfRangeException.GivenIndex}) given to the action ({exception.Action.Header}) " +
                    $"at index ({exception.ActionIndex}) for the template ({exception.Instruction.Header}) was out of bounds " +
                    $"of the arguments array. the arguments array only contains ({actionIndexOutOfRangeException.ArgumentCount}) " +
                    $"arguments" +
                    $"\n\n" +
                    $"this error is related to the defined Instruction set and not the assembly code. try editing the " +
                    $"instruction set and reloading. remember that indices are 0 based and do not start at 1");
                break;
            
            case NotEnoughActionIndicesException notEnoughActionIndicesException :
                ShowError($"(Line {exception.Instruction.ScriptLine}) " +
                          "Not enough action indices provided", 
                    $"the action ({exception.Action.Header}) at index ({exception.ActionIndex}) for the template " +
                    $"({exception.Instruction.Header}) was not given enough indices to perform its intended action. " +
                    $"Expected count : ({notEnoughActionIndicesException.ExpectedCount}) | Given count : {notEnoughActionIndicesException.GivenCount}" +
                    $"\n\n" +
                    $"this error is related to the defined Instruction set and not the assembly code. try editing the " +
                    $"instruction set and reloading.");
                break;
            
            case UnsupportedArgumentTypeException unsupportedArgumentTypeException :
                ShowError($"(Line {exception.Instruction.ScriptLine}) " +
                          "Unsupported argument type", 
                    $"the argument ({unsupportedArgumentTypeException.Argument}) does not match the argument type needed " +
                    $"for the action ({exception.Action.Header}) at index ({exception.ActionIndex}). try changing the " +
                    $"\n\n" +
                    $"this error is related to the defined Instruction set and not the assembly code. try changing the " +
                    $"argument type in the instruction set and reloading. if the problem persists, check the source code " +
                    $"on github and see what argument types can be used");
                break;
        }
    }
    public void ShowError(string title, string msg, double x = 0.5d, double y = 0.5d)
    {
        ErrorBox.Pop(x, y, title, msg);
    }
}