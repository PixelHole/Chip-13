using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Instructions;
using UI.SyntaxBox;

namespace Chippie_Lite_WPF.UI.Elements.InstructionDisplay;

public partial class InstructionDisplayItem : UserControl
{
    public Brush EvenLineBackground { get; set; }
    public Brush OddLineBackground { get; set; }
    
    
    public InstructionDisplayItem(Instruction? instruction = null, int index  = 0, SyntaxConfig? highlightConfig = null)
    {
        InitializeComponent();
        FetchColors();
        SetLineColor(index);
        if (instruction != null) SetDisplayData(instruction.Value, index);
        if (highlightConfig != null) SyntaxBox.GetSyntaxDrivers(InstructionBox).Add(highlightConfig);
    }

    private void FetchColors()
    {
        EvenLineBackground = (Application.Current.Resources["Dark Blue"] as Brush)!;
        OddLineBackground = (Application.Current.Resources["Faded Purple"] as Brush)!;
    }
    
    public void SetLineColor(int lineIndex)
    {
        SetBodyColor(lineIndex % 2 == 0 ? EvenLineBackground : OddLineBackground);
    }
    
    private void SetDisplayData(Instruction instruction, int index)
    {
        LineLabel.Content = index;
        InstructionBox.Text = instruction.ToString();
    }
    
    private void SetBodyColor(Brush color)
    {
        Body.Background = color;
    }
}