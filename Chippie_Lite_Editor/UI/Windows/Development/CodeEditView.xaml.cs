using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using UI.SyntaxBox;
using yt_unzipper;

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
        UpdateSetComboBox();
        ConnectEvents();
    }
    private void SetupSyntaxHighlighting()
    {
        var headers = InstructionSetManager.ActiveInstructionSet.GetHeadersCsv();
        
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

        RegexRule comment = new RegexRule()
        {
            Pattern = "#.*",
            Foreground = Application.Current.Resources["Faded Purple"] as Brush,
            Op = DriverOperation.Line
        };
        
        RegexRule label = new RegexRule()
        {
            Pattern = "@([A-Za-z]|[0-9])+",
            Foreground = Application.Current.Resources["Bisque"] as Brush,
            Op = DriverOperation.Line
        };
        
        SyntaxConfig config = [headersRule, registersRule, comment, label];

        SyntaxBox.GetSyntaxDrivers(InputBox).Add(config);
    }
    private void ConnectEvents()
    {
        InstructionSetManager.OnActiveSetChanged += OnOnActiveInstructionSetChanged;
    }
    private void OnOnActiveInstructionSetChanged(InstructionSet set, int index)
    {
        SetupSyntaxHighlighting();
    }

    public void SetEditable(bool editable)
    {
        WPFUtility.DoUiAction(InputBox, () => InputBox.IsEnabled = editable);
    }
    
    private void ZoomIn(int amount) => WPFUtility.DoUiAction(this, () => FontSize += amount);
    private void ZoomOut(int amount) => WPFUtility.DoUiAction(this, () => FontSize -= amount);
    private void ResetZoom() => WPFUtility.DoUiAction(this, () => FontSize = 32);
    
    private void UpdateLineDisplay(string[] lines)
    {
        WPFUtility.SetText(LineDisplay, "");

        int lineIndex = 1;
        
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                WPFUtility.AppendText(LineDisplay, ".\n");
                continue;
            }

            WPFUtility.AppendText(LineDisplay, lineIndex.ToString() + "\n");
            lineIndex++;
        }
    }
    private void UpdateLineCount(string[] lines)
    {
        int count = lines.Count(line => !string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#'));
        WPFUtility.SetContent(LineCountDisplay, $"LOC : {count}");
    }
    private void UpdateSetComboBox()
    {
        WPFUtility.DoUiAction(InstructionSetCB, UpdateComboBoxAction);
    }
    private void UpdateComboBoxAction()
    {
        var names = InstructionSetManager.GetSetNames();

        for (int i = 0; i < names.Length; i++)
        {
            if (i < InstructionSetCB.Items.Count)
            {
                var item = InstructionSetCB.Items[i] as ComboBoxItem;
                item.Content = names[i];
                InstructionSetCB.Items[i] = item;
            }
            else
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = names[i]
                };
                InstructionSetCB.Items.Add(item);
            }
        }

        for (int i = names.Length; i < InstructionSetCB.Items.Count; i++)
        {
            InstructionSetCB.Items.RemoveAt(InstructionSetCB.Items.Count - 1);
        }
    }

    public string[] GetInputLines()
    {
        return WPFUtility.GetText(InputBox).Split("\r\n");
    }

    public Point HighlightLine(int lineIndex)
    {
        WPFUtility.DoUiAction(InputBox, () => InputBox.ScrollToLine(lineIndex));
        int start = 0, length = 0;
        WPFUtility.DoUiAction(InputBox, () => start = InputBox.GetCharacterIndexFromLineIndex(lineIndex));
        WPFUtility.DoUiAction(InputBox, () => length = InputBox.GetLineLength(lineIndex));
        InputBox.Select(start, length);

        Rect rect = Rect.Empty;
        WPFUtility.DoUiAction(InputBox, () => rect = InputBox.GetRectFromCharacterIndex(start + length - 1));
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
    private void InstructionSetSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }
}