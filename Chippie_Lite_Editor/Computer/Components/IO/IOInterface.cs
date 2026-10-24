using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Utility;
using wpf_Console;

namespace Chippie_Lite_WPF.Computer.Components;

public static class IOInterface
{
    private static Vector2Int displaySize = new(32, 16);


    public static Vector2Int DisplaySize
    {
        get => displaySize;
        set
        {
            if (displaySize == value) return;
            displaySize = value;
            DisplaySizeSet?.Invoke(value);
        }
    }

    public delegate void SetDisplaySizeAction(Vector2Int size);
    public static event SetDisplaySizeAction? DisplaySizeSet;
    
    public delegate void ConsoleBackgroundSetAction(int index);
    public static event ConsoleBackgroundSetAction? SetConsoleBackgroundRequest;
    
    public delegate void BackgroundSetAction(int index);
    public static event BackgroundSetAction? SetBackgroundRequest;

    public delegate void ForegroundSetAction(int index);
    public static event ForegroundSetAction? SetForegroundRequest;

    public delegate void CursorSetAction(Vector2Int position);
    public static event CursorSetAction? SetCursorRequest;
    
    public delegate void CursorSetLeftAction(int left);
    public static event CursorSetLeftAction? SetCursorLeftRequest;
    
    public delegate void CursorSetTopAction(int top);
    public static event CursorSetTopAction? SetCursorTopRequest;
    
    public delegate void ConsoleClearAction();
    public static event ConsoleClearAction? ClearConsoleRequest;
    
    public static void SetCursor(Vector2Int cursor) => SetCursorRequest?.Invoke(cursor);
    public static void SetCursorLeft(int left) => SetCursorLeftRequest?.Invoke(left);
    public static void SetCursorTop(int top) => SetCursorTopRequest?.Invoke(top);

    public static void SetConsoleBackgroundIndex(int index) => SetConsoleBackgroundRequest?.Invoke(index);
    public static void SetForegroundIndex(int index) => SetForegroundRequest?.Invoke(index);
    public static void SetBackgroundIndex(int index) => SetBackgroundRequest?.Invoke(index);

    public static void ClearConsole() => ClearConsoleRequest?.Invoke();
}