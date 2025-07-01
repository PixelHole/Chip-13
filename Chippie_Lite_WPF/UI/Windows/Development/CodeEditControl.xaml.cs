using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class CodeEditControl : UserControl
{
    public CodeEditControl()
    {
        InitializeComponent();
        ConnectEvents();
    }

    private void UpdateLineDisplay(string[] lines)
    {
        LineDisplay.Text = "";

        int lineIndex = 1;
        
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
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
    
    private void ConnectEvents()
    {
        InputBox.TextChanged += InputBoxOnTextChanged;
    }
    
    private void InputBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        var lines = GetInputLines();
        UpdateLineDisplay(lines);
        UpdateLineCount(lines);
    }
}