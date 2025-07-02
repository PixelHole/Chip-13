using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions.Templates;
using UI.SyntaxBox;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class CodeEditControl : UserControl
{
    public string InputText => InputBox.Text;
    
    
    public CodeEditControl()
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
            Foreground = Application.Current.Resources["Green"] as Brush,
            WholeWordsOnly = true,
        };
        
        SyntaxConfig config = [headersRule, registersRule];

        SyntaxBox.GetSyntaxDrivers(InputBox).Add(config);
    }
    private void ConnectEvents()
    {
        InputBox.TextChanged += InputBoxOnTextChanged;
    }

    public void SetEditable(bool editable)
    {
        InputBox.IsEnabled = editable;
    }
    
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
        int count = lines.Count(line => !string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line));
        LineCountDisplay.Content = $"LOC : {count}";
    }

    public string[] GetInputLines()
    {
        return InputBox.Text.Split("\r\n");
    }

    public void HighlightLine(int lineIndex)
    {
        InputBox.ScrollToLine(lineIndex);
        int start = InputBox.GetCharacterIndexFromLineIndex(lineIndex);
        int length = InputBox.GetLineLength(lineIndex);
        InputBox.Select(start, length);
    }
    
    private void InputBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        var lines = GetInputLines();
        UpdateLineDisplay(lines);
        UpdateLineCount(lines);
    }
}