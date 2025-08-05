using System.IO;
using System.Text.Json;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.Computer.File;

public static class ChippeExceptionHandler
{
    public static void HandleInitException(Exception e)
    {
        switch (e)
        {
            case FileOpenException fileOpenException :
                HandleISFileOpenException(fileOpenException.ActualException);
                return;
            
            case FileReadException fileReadException :
                HandleISFileReadException(fileReadException.ActualException);
                return;
            
            case FileParseException fileParseException :
                HandleISFileParseException(fileParseException);
                return;
            
            case RegisterNotFoundException:
                ErrorBox.Pop("Instruction pointer register not found",
                    $"make sure an Instruction pointer register is defined");
                return;
            
            default :
                ErrorBox.Pop("Uh oh.", "an unknown error has occured while loading the file." +
                                       "try restarting the program and check that the file is formatted correctly");
                return;
        }
    }

    private static void HandleISFileOpenException(Exception actualException)
    {
        string title = "Error loading instruction set";
        
        switch (actualException)
        {
            case ArgumentException or ArgumentNullException:
                ErrorBox.Pop(title, "the given path was null");
                return;
            
            case FileNotFoundException or DirectoryNotFoundException :
                ErrorBox.Pop(title, "the chosen file or directory could not be found");
                return;
            
            case IOException:
                ErrorBox.Pop(title, "the selected path contains one or more errors in either file or directory names");
                return;
            
            default :
                ErrorBox.Pop(title, "an unknown error has occured while trying to open the instruction set. please try again");
                return;
        }
    }
    private static void HandleISFileReadException(Exception actualException)
    {
        string title = "Error reading instruction set";
        
        switch (actualException)
        {
            case OutOfMemoryException:
                ErrorBox.Pop(title, "there wasn't enough memory in your device to fully read the file");
                return;
            
            case IOException :
                ErrorBox.Pop(title, "could not read the chosen file. this can be because the file is open in another process");
                return;
            
            default :
                ErrorBox.Pop(title, "an unknown error has occured while reading the instruction set file. please try again");
                return;
        }
    }
    private static void HandleISFileParseException(Exception actualException)
    {
        string title = "Error parsing instruction set";
        
        switch (actualException)
        {
            case ArgumentNullException:
                ErrorBox.Pop(title, "the given data was null");
                return;
            
            case JsonException :
                ErrorBox.Pop(title, "the chosen json file was not formatted correctly");
                return;
            
            case NotSupportedException :
                ErrorBox.Pop(title, "one or more elements could not be deserialized from json. make sure they are named correctly");
                return;
            
            default :
                ErrorBox.Pop(title, "an unknown error has occured while parsing the loaded data. please try again");
                return;
        }
    }
}