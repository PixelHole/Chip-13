using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.Computer.File;

public static class InstructionExceptionHandler
{
    public static void HandleInstructionException(Exception e)
    {
        switch (e)
        {
            case InvalidInstructionException invalidInstructionException :
                HandleInvalidInstructionException(invalidInstructionException);
                return;
            
            case InstructionInterpretationException interpretationException :
                HandleInterpretationException(interpretationException);
                return;
            
            default :
                ErrorBox.Pop("Uh oh.",
                    "an unknown error has occured. it's probably fine " +
                    "but we suggest saving your code and restarting the app");
                return;
        }
    }
    private static void HandleInvalidInstructionException(InvalidInstructionException exception)
    {
        switch (exception)
        {
            case RegisterNotFoundException registerNotFoundException:
                ErrorBox.Pop($"(Line {exception.ActualLine + 1}) : Register not found",
                    $"the stated register with id ({registerNotFoundException.ProvidedId}) could not be found. " +
                    $"please make sure you have spelled the register id correctly");
                return;
            case InvalidHeaderException headerException:
                ErrorBox.Pop($"(Line {exception.ActualLine + 1}) : Invalid instruction header", 
                    $"invalid instruction header ({headerException.ProvidedHeader}). make sure the header is spelled correctly." +
                    $" if correct, it will take on a pink color");
                return;
            case NotEnoughArgumentsException notEnoughArgumentsException:
                ErrorBox.Pop($"Line {exception.ActualLine + 1}) : Not enough arguments",
                    $"instruction was not given enough arguments. it needs ({notEnoughArgumentsException.ExpectedCount})." +
                    $"you have given it ({notEnoughArgumentsException.ProvidedCount})");
                return;
            
            case InvalidArgumentException argumentException:
                ErrorBox.Pop($"Line {exception.ActualLine + 1}) : Invalid argument",
                    $"argument string ({argumentException.InputString}) does not match the type ({argumentException.ExpectedType})." +
                    $" make sure you have formatted your argument correctly and is the correct type." +
                    $" if so, the string will likely change color");
                return;
            
            default :
                ErrorBox.Pop($"[{exception.ActualLine}] uh oh.", "an unknown error has occured trying to parse the " +
                                                                 $"instruction at line [{exception.ActualLine}]");
                return;
        }
    }

    private static void HandleInterpretationException(InstructionInterpretationException exception)
    {
        switch (exception)
        {
            case ActionIndexOutOfRangeException actionIndexOutOfRangeException :
                ErrorBox.Pop($"(Line {exception.Instruction.ScriptLine}) " +
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
                ErrorBox.Pop($"(Line {exception.Instruction.ScriptLine}) " +
                             "Not enough action indices provided", 
                    $"the action ({exception.Action.Header}) at index ({exception.ActionIndex}) for the template " +
                    $"({exception.Instruction.Header}) was not given enough indices to perform its intended action. " +
                    $"Expected count : ({notEnoughActionIndicesException.ExpectedCount}) | Given count : {notEnoughActionIndicesException.GivenCount}" +
                    $"\n\n" +
                    $"this error is related to the defined Instruction set and not the assembly code. try editing the " +
                    $"instruction set and reloading.");
                break;
            
            case UnsupportedArgumentTypeException unsupportedArgumentTypeException :
                ErrorBox.Pop($"(Line {exception.Instruction.ScriptLine}) " +
                             "Unsupported argument type", 
                    $"the argument ({unsupportedArgumentTypeException.Argument}) does not match the argument type needed " +
                    $"for the action ({exception.Action.Header}) at index ({exception.ActionIndex}). try changing the " +
                    $"\n\n" +
                    $"this error is related to the defined Instruction set and not the assembly code. try changing the " +
                    $"argument type in the instruction set and reloading. if the problem persists, check the source code " +
                    $"on github and see what argument types can be used");
                break;
            
            default :
                ErrorBox.Pop($"[{exception.Instruction.ScriptLine}] uh oh.",
                    $"unknown error occured while trying to interpret instruction [{exception.Instruction.ToString()}]" +
                    $"at line [{exception.Instruction.ScriptLine}]");
                return;
        }
    }
}