using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class MemoryViewCell : UserControl
{
    private string LastSafeInput = "";
    
    public delegate void TextChangedAction(MemoryViewCell sender, int data);
    public event TextChangedAction? OnTextChanged;

    private Brush InvalidTextBrush;
    private Brush ValidTextBrush;
    
    public MemoryViewCell()
    {
        InitializeComponent();
        FetchColors();
    }
    private void FetchColors()
    {
        InvalidTextBrush = (Application.Current.Resources["Red"] as Brush)!;
        ValidTextBrush = (Application.Current.Resources["White"] as Brush)!;
    }
    
    public void SetText(string text)
    {
        if (Dispatcher.CheckAccess())
        {
            InputBox.Text = text;
            return;
        }

        Dispatcher.Invoke(() => InputBox.Text = text);
    }
    public void SetAddressText(string text)
    {
        if (Dispatcher.CheckAccess())
        {
            AddressBlock.Text = text;
            return;
        }

        Dispatcher.Invoke(() => AddressBlock.Text = text);
    }
    public string GetText()
    {
        if (Dispatcher.CheckAccess())
        {
            return InputBox.Text;
        }

        string text = "";

        Dispatcher.Invoke(() => text = InputBox.Text);

        return text;
    }
    
    private void UpdateTextRequest()
    {
        int data = 0;

        try
        {
            data =  ParseInput();
        }
        catch (Exception)
        {
            SetText(LastSafeInput);
        }

        OnTextChanged?.Invoke(this, data);
    }

    private int ParseInput()
    {
        string text = GetText();

        var num = StringUtility.IsStringOrChar(text) ? StringUtility.TextToInt(text) : NumberUtility.ParseNumber(text);

        return num;
    }

    public void SetControlMode(ControlMode mode)
    {
        if (Dispatcher.CheckAccess())
        {
            InputBox.IsEnabled = mode == ControlMode.Edit;
            return;
        }

        Dispatcher.Invoke(() =>
        {
            InputBox.IsEnabled = mode == ControlMode.Edit;
        });
    }
    
    private void InputBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (!InputBox.IsFocused) return;
        
        UpdateTextRequest();
    }
    private void InputBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                Keyboard.ClearFocus();
                return;
            
            case Key.Enter:
                UpdateTextRequest();
                return;
            
            default:
                return;
        }
    }
    private void InputBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var input = GetText();
        
        if (IsInputValid(input))
        {
            InputBox.Foreground = ValidTextBrush;
            LastSafeInput = input;
            return;
        }
        
        InputBox.Foreground = InvalidTextBrush;
    }

    private bool IsInputValid(string input)
    {
        return StringUtility.IsStringOrChar(input) || NumberUtility.TryParseNumber(input, out _);
    }
}