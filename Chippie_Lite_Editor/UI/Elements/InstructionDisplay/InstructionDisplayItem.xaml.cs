using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Instructions;
using UI.SyntaxBox;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class InstructionDisplayItem : UserControl
{
    public Brush HighlightedBackground { get; set; } = null!;


    public InstructionDisplayItem(Instruction? instruction = null, int index  = 0, SyntaxConfig? highlightConfig = null)
    {
        InitializeComponent();
        FetchColors();
        if (instruction != null) SetDisplayData(instruction.Value, index);
        if (highlightConfig != null) SyntaxBox.GetSyntaxDrivers(InstructionBox).Add(highlightConfig);
    }

    private void FetchColors()
    {
        HighlightedBackground = (Application.Current.Resources["Pink"] as Brush)!;
    }
    
    public void SetHighlighted(bool highlighted)
    {
        Body.Background = highlighted ? HighlightedBackground : new SolidColorBrush(Colors.Transparent);
    }
    
    private void SetDisplayData(Instruction instruction, int index)
    {
        LineLabel.Content = instruction.HasBreakpoint ? $"{index}!" : index;
        InstructionBox.Text = instruction.ToString();
    }
    
    private void SetBodyColor(Brush color)
    {
        Body.Background = color;
    }
}