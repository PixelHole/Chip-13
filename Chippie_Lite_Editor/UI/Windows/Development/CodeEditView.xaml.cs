using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using UI.SyntaxBox;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class CodeEditView
{
    private double fontSize = 32;
    
    public string InputText
    {
        get
        {
            string text = "";

            if (InputBox.CheckAccess())
            {
                text = InputBox.Text;
            }
            else
            {
                InputBox.Dispatcher.Invoke(() => text = InputBox.Text);
            }

            return text;
        }
        set
        {
            if (InputBox.CheckAccess())
            {
                InputBox.Text = value;
            }
            else
            {
                InputBox.Dispatcher.Invoke(() => InputBox.Text = value);
            }
        }
    }
    public new double FontSize
    {
        get => fontSize;
        set
        {
            bool same = Math.Abs(fontSize - value) < 0.1d;
            if (same || value < 8) return;
            fontSize = value;
            if (CheckAccess())
            {
                InputBox.FontSize = fontSize;
                LineDisplay.FontSize = fontSize;
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    InputBox.FontSize = fontSize;
                    LineDisplay.FontSize = fontSize;
                });
            }
        }
    }

    public Key ZoomResetKey { get; set; } = Key.Q;
    
    
    public CodeEditView()
    {
        InitializeComponent();
        SetupSyntaxHighlighting();
        ConnectEvents();
    }
    private void SetupSyntaxHighlighting()
    {
        var headers = InstructionSet.GetHeadersCsv();
        
        KeywordRule headersRule = new KeywordRule
        {
            Keywords = headers,
            Foreground = Application.Current.Resources["Pink"] as Brush,
            WholeWordsOnly = true,
        };

        var registers = RegisterBank.GetRegistersCsv();
        
        KeywordRule registersRule = new KeywordRule()
        {
            Keywords = registers,
            Foreground = Application.Current.Resources["Dark Green"] as Brush,
            WholeWordsOnly = true,
        };
        
        SyntaxConfig config = [headersRule, registersRule];

        SyntaxBox.GetSyntaxDrivers(InputBox).Add(config);
    }
    private void ConnectEvents()
    {
        
    }

    public void SetEditable(bool editable)
    {
        InputBox.IsEnabled = editable;
    }
    
    private void ZoomIn(int amount)
    {
        FontSize += amount;
    }
    private void ZoomOut(int amount)
    {
        FontSize -= amount;
    }
    private void ResetZoom() => FontSize = 32;
    
    private void UpdateLineDisplay(string[] lines)
    {
        LineDisplay.Text = "";

        int lineIndex = 1;
        
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                LineDisplay.Text += ".\n";
                continue;
            }

            LineDisplay.Text += lineIndex.ToString() + "\n";
            lineIndex++;
        }
    }
    private void UpdateLineCount(string[] lines)
    {
        int count = lines.Count(line => !string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#'));
        LineCountDisplay.Content = $"LOC : {count}";
    }

    public string[] GetInputLines()
    {
        return InputBox.Text.Split("\r\n");
    }

    public Point HighlightLine(int lineIndex)
    {
        InputBox.ScrollToLine(lineIndex);
        int start = InputBox.GetCharacterIndexFromLineIndex(lineIndex);
        int length = InputBox.GetLineLength(lineIndex);
        InputBox.Select(start, length);

        var rect = InputBox.GetRectFromCharacterIndex(start + length - 1);
        return rect.BottomRight;
    }
    
    private void InputBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        var lines = GetInputLines();
        UpdateLineDisplay(lines);
        UpdateLineCount(lines);
    }
    private void InputBox_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.IsKeyUp(Key.LeftCtrl) && Keyboard.IsKeyUp(Key.RightCtrl)) return;
        
        switch (e.Delta)
        {
            case > 0:
                ZoomIn(4);
                break;
            case < 0:
                ZoomOut(4);
                break;
        }
    }
    private void InputBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == ZoomResetKey && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift)) ResetZoom();
    }
}