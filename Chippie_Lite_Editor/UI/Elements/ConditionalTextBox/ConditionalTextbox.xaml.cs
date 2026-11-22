using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;
using Timer = System.Timers.Timer;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ConditionalTextbox
{
    public static readonly DependencyProperty RetainTextProperty = DependencyProperty.Register(nameof(RetainText), typeof(bool), typeof(ConditionalTextbox), new PropertyMetadata(null)); 
    public static readonly DependencyProperty ValidBrushProperty = DependencyProperty.Register(nameof(ValidTextBrush), typeof(Brush), typeof(ConditionalTextbox), new PropertyMetadata(null)); 
    public static readonly DependencyProperty InvalidBrushProperty = DependencyProperty.Register(nameof(InvalidTextBrush), typeof(Brush), typeof(ConditionalTextbox), new PropertyMetadata(null));
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(ConditionalTextbox), new PropertyMetadata(string.Empty));
    
    public required Predicate<string> Condition
    {
        set => validatorBuffer = new ValidatedBuffer<string>(value)!;
    }
    
    private ValidatedBuffer<string?> validatorBuffer { get; set; } = new(_ => true);

    private readonly Timer ConfirmTimer = new();

    public string Text
    {
        get => WPFUtility.GetText(ContentBox);
        set
        {
            validatorBuffer.Set(value);
            WPFUtility.SetText(ContentBox, validatorBuffer.Get() ?? "");
        }
    }
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    
    public bool RetainText
    {
        get => (bool)GetValue(RetainTextProperty);
        set => SetValue(RetainTextProperty, value);
    }

    public Brush? ValidTextBrush
    {
        get => (Brush?)GetValue(ValidBrushProperty);
        set => SetValue(ValidBrushProperty, value);
    }

    public Brush? InvalidTextBrush
    {
        get => (Brush?)GetValue(InvalidBrushProperty);
        set => SetValue(InvalidBrushProperty, value);
    }
    
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
    
    private void TryConfirmText()
    {
        if (validatorBuffer.Set(ContentBox.Text)) TextConfirmed?.Invoke(ContentBox.Text);
        else if (!RetainText) ContentBox.Text = validatorBuffer.Get();
    }

    private void StartConfirmTimer()
    {
        ConfirmTimer.Stop();
        ConfirmTimer.Interval = 500d;
        ConfirmTimer.Elapsed += (_, _) => TryConfirmText();
        ConfirmTimer.Start();
    }
    
    private void ContentBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ContentBox.Foreground = validatorBuffer.Rule.Invoke(ContentBox.Text) ? ValidTextBrush : InvalidTextBrush;
        StartConfirmTimer();
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
    private void ContentBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        
    }
}