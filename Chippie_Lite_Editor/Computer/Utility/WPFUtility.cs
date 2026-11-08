using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace yt_unzipper;

public static class WPFUtility
{
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

    public static void SetToolTip(FrameworkElement element, object tooltip)
    {
        DoUiAction(element, () => element.ToolTip = tooltip);
    }
    
    public static void DoUiAction(UIElement element, Action action)
    {
        if (element.CheckAccess()) action.Invoke();
        else element.Dispatcher.Invoke(action);
    }
}