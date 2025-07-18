using System.Windows;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.Controls;

public class ErrorBoxManager
{
    public static void ShowExceptionError(InvalidInstructionException exception, Point point)
    {
        switch (exception)
        {
            case RegisterNotFoundException registerNotFoundException:
                ShowError($"(Line {exception.ActualLine + 1}) : Register not found",
                    $"the stated register with id ({registerNotFoundException.ProvidedId}) could not be found. " +
                    $"please make sure you have spelled the register id correctly",
                    point);
                return;
            case InvalidHeaderException headerException:
                ShowError($"(Line {exception.ActualLine + 1}) : Invalid instruction header", 
                    $"invalid instruction header ({headerException.ProvidedHeader}). make sure the header is spelled correctly." +
                    $" if correct, it will take on a pink color",
                    point);
                return;
            case NotEnoughArgumentsException notEnoughArgumentsException:
                ShowError($"Line {exception.ActualLine + 1}) : Not enough arguments",
                    $"instruction was not given enough arguments. it needs ({notEnoughArgumentsException.ExpectedCount})." +
                    $"you have given it ({notEnoughArgumentsException.ProvidedCount})",
                    point);
                return;
            
            case InvalidArgumentException argumentException:
                ShowError($"Line {exception.ActualLine + 1}) : Invalid argument",
                    $"argument string ({argumentException.InputString}) does not match the type ({argumentException.ExpectedType})." +
                    $" make sure you have formatted your argument correctly and is the correct type." +
                    $" if so, the string will likely change color",
                    point);
                return;
        }
    }
    public static void ShowExceptionError(InstructionInterpretationException exception)
    {
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
    public static void ShowError(string title, string msg, Point point = default)
    {
        ErrorBox.Pop(title, msg, point);
    }
}