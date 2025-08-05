using System.IO;
using System.Security;
using System.Text.Json;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.Computer.File;

public static class SaveFileExceptionHandler
{
    public static void HandleLoadException(Exception e)
    {
        switch (e)
        {
            case FileOpenException fileOpenException :
                HandleLoadFileOpenException(fileOpenException.ActualException);
                return;
            
            case FileReadException fileReadException :
                HandleLoadFileReadException(fileReadException.ActualException);
                return;
            
            case FileParseException fileParseException :
                HandleLoadFileParseException(fileParseException);
                return;
            
            case RegisterNotFoundException registerNotFoundException:
                ErrorBox.Pop("Register not found",
                    $"register with name {registerNotFoundException.ProvidedId} could not be found");
                return;
            
            default :
                ErrorBox.Pop("Uh oh.", "an unknown error has occured while loading the file." +
                                       "try restarting the program and check that the file is formatted correctly");
                return;
        }
    }

    private static void HandleLoadFileOpenException(Exception actualException)
    {
        string title = "Error loading data";
        
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
                ErrorBox.Pop(title, "an unknown error has occured while loading the file. please try again");
                return;
        }
    }
    private static void HandleLoadFileReadException(Exception actualException)
    {
        string title = "Error reading data";
        
        switch (actualException)
        {
            case OutOfMemoryException:
                ErrorBox.Pop(title, "there wasn't enough memory in your device to fully read the file");
                return;
            
            case IOException :
                ErrorBox.Pop(title, "could not read the chosen file. this can be because the file is open in another process");
                return;
            
            default :
                ErrorBox.Pop(title, "an unknown error has occured while reading the file. please try again");
                return;
        }
    }
    private static void HandleLoadFileParseException(Exception actualException)
    {
        string title = "Error parsing data";
        
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


    public static void HandleSaveExceptions(Exception e)
    {
        switch (e)
        {
            case FileOpenException fileOpenException :
                HandleSaveFileOpenException(fileOpenException.ActualException);
                return;
            
            case FileWriteException fileWriteException :
                HandleSaveFileWriteException(fileWriteException.ActualException);
                return;
            
            default:
                ErrorBox.Pop("Uh oh.", "an unknown error has occured trying to save this instance." +
                                       "please try again and if this consists, issue a bug report on the github page");
                return;
        }
    }

    private static void HandleSaveFileOpenException(Exception actualException)
    {
        string title = "Error writing file";
        
        switch (actualException)
        {
            case UnauthorizedAccessException :
                ErrorBox.Pop(title, "this application does not have access to this file");
                return;
            
            case ArgumentException or ArgumentNullException :
                ErrorBox.Pop(title, "the given path is either null or invalid");
                return;
            
            case DirectoryNotFoundException :
                ErrorBox.Pop(title, "one or more directories in the path could not be found");
                return;
            
            case PathTooLongException :
                ErrorBox.Pop(title, "the chosen path is too long and cannot be opened. " +
                                    "consider moving the file to a location with a shorter path");
                return;
            
            case IOException :
                ErrorBox.Pop(title,
                    "path has an incorrect or invalid syntax for file name, directory name, or volume label syntax.");
                return;
            
            case SecurityException :
                ErrorBox.Pop(title, "this application does not have the permission to open this file");
                return;
            
            default :
                ErrorBox.Pop(title, "an unknown error has occured while opening the file. please try again");
                return;
        }
    }
    private static void HandleSaveFileWriteException(Exception actualException)
    {
        string title = "Error writing data";
        
        switch (actualException)
        {
            case ObjectDisposedException :
                ErrorBox.Pop(title, "Stream writer closed sooner than expected, please try saving again");
                return;
            
            case NotSupportedException :
                ErrorBox.Pop(title,
                    "the StreamWriter buffer is full, and the contents of the buffer cannot be written " +
                    "to the underlying fixed size stream because the StreamWriter is at the end the stream");
                return;
            
            case IOException :
                ErrorBox.Pop(title, "could not write to the selected file, this could be because the file is open" +
                                    "in another process");
                return;
            
            default :
                ErrorBox.Pop(title, "an unknown error has occured while writing to the file. please try again");
                return;
        }
    }
}