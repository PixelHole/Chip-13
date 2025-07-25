using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Controls.Utility;
using Timer = System.Timers.Timer;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ConditionalTextbox : UserControl
{
    public required Predicate<string> Condition
    {
        set => validatorBuffer = new ValidatedBuffer<string>(value);
    }
    private ValidatedBuffer<string> validatorBuffer { get; set; }

    public FontFamily FontFamily
    {
        get => ContentBox.FontFamily;
        set => ContentBox.FontFamily = value;
    }
    public double FontSize
    {
        get => ContentBox.FontSize;
        set => ContentBox.FontSize = value;
    }
    public bool IsEnabled
    {
        get => ContentBox.IsEnabled;
        set => ContentBox.IsEnabled = value;
    }

    public double BufferSetDelay { get; set; }
    private bool HasTimer => BufferSetDelay != 0;
    private Timer SetTimer = new();
    
    public bool RetainText { get; set; } = true;
    
    public Brush? ValidTextBrush { get; set; }
    public Brush? InvalidTextBrush { get; set; }

    public delegate void TextConfirmAction(string text);
    public event TextConfirmAction? TextConfirmed;
    
    public ConditionalTextbox()
    {
        InitializeComponent();
        FetchColors();
    }
    private void FetchColors()
    {
        ValidTextBrush ??= Application.Current.Resources["White"] as Brush;
        InvalidTextBrush ??= Application.Current.Resources["White"] as Brush;
    }

    public void SetText(string text)
    {
        validatorBuffer.Set(text);
        
        if (Dispatcher.CheckAccess())
        {
            ContentBox.Text = validatorBuffer.Get();
        }
        else
        {
            Dispatcher.Invoke(() => ContentBox.Text = validatorBuffer.Get());
        }
    }
    public string GetText()
    {
        string text = "";

        if (Dispatcher.CheckAccess())
        {
            text = ContentBox.Text;
        }
        else
        {
            Dispatcher.Invoke(() => text = ContentBox.Text);
        }

        return text;
    }


    private void TryConfirmText()
    {
        if (validatorBuffer.Set(ContentBox.Text)) TextConfirmed?.Invoke(ContentBox.Text);
        else if (!RetainText) ContentBox.Text = validatorBuffer.Get();
    }
    
    private void ContentBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ContentBox.Foreground = validatorBuffer.Rule.Invoke(ContentBox.Text) ? ValidTextBrush : InvalidTextBrush;
    }
    private void ContentBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is not (Key.Escape or Key.Enter)) return;
        Keyboard.ClearFocus();
        TryConfirmText();
    }
    private void ContentBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        TryConfirmText();
    }
}