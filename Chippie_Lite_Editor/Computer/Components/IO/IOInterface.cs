using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Utility;
using wpf_Console;

namespace Chippie_Lite_WPF.Computer.Components;

public class IOInterface : ChippieComponent
{
    private Vector2Int displaySize = new(32, 16);


    public Vector2Int DisplaySize
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
    public event SetDisplaySizeAction? DisplaySizeSet;
    
    public delegate void ConsoleBackgroundSetAction(int index);
    public event ConsoleBackgroundSetAction? SetConsoleBackgroundRequest;
    
    public delegate void BackgroundSetAction(int index);
    public event BackgroundSetAction? SetBackgroundRequest;

    public delegate void ForegroundSetAction(int index);
    public event ForegroundSetAction? SetForegroundRequest;

    public delegate void CursorSetAction(Vector2Int position);
    public event CursorSetAction? SetCursorRequest;
    
    public delegate void CursorSetLeftAction(int left);
    public event CursorSetLeftAction? SetCursorLeftRequest;
    
    public delegate void CursorSetTopAction(int top);
    public event CursorSetTopAction? SetCursorTopRequest;
    
    public delegate void ConsoleClearAction();
    public event ConsoleClearAction? ClearConsoleRequest;
    
    public void SetCursor(Vector2Int cursor) => SetCursorRequest?.Invoke(cursor);
    public void SetCursorLeft(int left) => SetCursorLeftRequest?.Invoke(left);
    public void SetCursorTop(int top) => SetCursorTopRequest?.Invoke(top);

    public void SetConsoleBackgroundIndex(int index) => SetConsoleBackgroundRequest?.Invoke(index);
    public void SetForegroundIndex(int index) => SetForegroundRequest?.Invoke(index);
    public void SetBackgroundIndex(int index) => SetBackgroundRequest?.Invoke(index);

    public void ClearConsole() => ClearConsoleRequest?.Invoke();
}