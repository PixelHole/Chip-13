using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class MemoryViewCell : UserControl
{
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
    public void SetText(string text) => InputBox.SetText(text);
    public string GetText() => InputBox.GetText();
    public void SetAddressText(string text)
    {
        if (Dispatcher.CheckAccess())
        {
            AddressBlock.Text = text;
            return;
        }

        Dispatcher.Invoke(() => AddressBlock.Text = text);
    }
    
    private void PassTextToList()
    {
        int data =  ParseInput();
        OnTextChanged?.Invoke(this, data);
    }
    private int ParseInput()
    {
        string text = GetText();

        var num = StringUtility.IsStringOrChar(text) ? StringUtility.TextToInt(text) : NumberUtility.ParseNumber(text);

        return num;
    }
    

    private bool IsInputValid(string input)
    {
        return StringUtility.IsStringOrChar(input) || NumberUtility.TryParseNumber(input, out _);
    }
    private void InputBox_OnTextConfirmed(string text)
    {
        PassTextToList();
    }
}