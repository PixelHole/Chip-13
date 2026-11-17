using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using Control = System.Windows.Controls.Control;
using TextBox = System.Windows.Controls.TextBox;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class WPFUtility
{
    [StructLayout(LayoutKind.Sequential)]
    private struct FLASHWINFO
    {
        public uint cbSize;
        public IntPtr hwnd;
        public uint dwFlags;
        public uint uCount;
        public uint dwTimeout;
    }
    
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
    
    // Flashing flags
    private const uint FLASHW_STOP = 0;          // Stop flashing
    private const uint FLASHW_CAPTION = 1;       // Flash the window title bar
    private const uint FLASHW_TRAY = 2;          // Flash the taskbar button
    private const uint FLASHW_ALL = 3;           // Flash both title bar and taskbar
    private const uint FLASHW_TIMER = 4;         // Flash continuously until stopped
    private const uint FLASHW_TIMERNOFG = 12;    // Flash continuously until the window comes to foreground

    public static void FlashWindow(Window window)
    {
        IntPtr handle = new WindowInteropHelper(window).Handle;
        
        FLASHWINFO flashInfo = new FLASHWINFO();
        flashInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(flashInfo));
        flashInfo.hwnd = handle;
        
        flashInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
        flashInfo.uCount = UInt32.MaxValue;
        flashInfo.dwTimeout = 0;

        FlashWindowEx(ref flashInfo);
    }
    
    public static string GetText(TextBox element)
    {
        string text = "";
        DoUiAction(element, () => text = element.Text);
        return text;
    }
    public static string GetText(TextBlock element)
    {
        string text = "";
        DoUiAction(element, () => text = element.Text);
        return text;
    }
    
    public static void SetText(TextBox element, string text)
    {
        DoUiAction(element, () => element.Text = text);
    }
    public static void SetText(TextBlock element, string text)
    {
        DoUiAction(element, () => element.Text = text);
    }

    public static void AppendText(TextBlock element, string text) => SetText(element, GetText(element) + text);
    public static void AppendText(TextBox element, string text) => SetText(element, GetText(element) + text);

    public static object? GetContent(ContentControl element)
    {
        object? text = null;
        DoUiAction(element, () => text = element.Content);
        return text;
    }
    public static void SetContent(ContentControl element, object content)
    {
        DoUiAction(element, () => element.Content = content);
    }

    public static void SetBackground(Control element, Brush color)
    {
        DoUiAction(element, () => element.Background = color);
    }
    public static Brush GetBackground(Control element)
    {
        Brush? bg = null;
        DoUiAction(element, () => bg = element.Background);
        return bg!;
    }
    public static void SetForeground(Control element, Brush color)
    {
        DoUiAction(element, () => element.Foreground = color);
    }
    public static Brush GetForeground(Control element)
    {
        Brush? bg = null;
        DoUiAction(element, () => bg = element.Foreground);
        return bg!;
    }

    public static void SetToolTip(FrameworkElement element, object tooltip)
    {
        DoUiAction(element, () => element.ToolTip = tooltip);
    }

    public static void CenterWindow(Window window)
    {
        window.WindowState = WindowState.Normal;
        SetWindowPosition(window, 0.5, 0.5);
    }
    public static void SetWindowPosition(Window window, double x, double y)
    {
        IntPtr windowHandle = new WindowInteropHelper(window).Handle;
        Screen screen = Screen.FromHandle(windowHandle);

        var area = screen.WorkingArea;

        var source = PresentationSource.FromVisual(window);
        double dpiX = source?.CompositionTarget?.TransformToDevice.M11 ?? 1d;
        double dpiY = source?.CompositionTarget?.TransformToDevice.M22 ?? 1d;
        
        double screenLeft = area.Left / dpiX;
        double screenTop = area.Top / dpiY;
        double screenWidth = area.Width / dpiX;
        double screenHeight = area.Height / dpiY;


        window.Left = screenLeft + (screenWidth - window.ActualWidth) * x;
        window.Top = screenTop + (screenHeight - window.ActualHeight) * y;
    }
    public static void BringWindowToTop(Window window)
    {
        window.WindowState = WindowState.Normal;
        window.Topmost = true;
    }

    public static void SetVisibility(UIElement element, Visibility visibility)
    {
        DoUiAction(element, () => element.Visibility = visibility);
    }
    
    public static void DoUiAction(UIElement element, Action action)
    {
        if (element.CheckAccess()) action.Invoke();
        else element.Dispatcher.Invoke(action);
    }
}