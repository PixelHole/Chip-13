using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.UI.Elements.InstructionDisplay;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class InstructionDisplayList : UserControl
{
    public InstructionDisplayList()
    {
        InitializeComponent();
    }

    public void SetListItems(IList<Instruction> instructions)
    {
        ClearInstructionList();
        for (int i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];
            AddInstructionToList(instruction, i);
        }
    }

    private void AddInstructionToList(Instruction instruction, int index)
    {
        InstructionDisplayItem item = new InstructionDisplayItem(instruction, index);
        InstructionList.Children.Add(item);
    }
    private void ClearInstructionList()
    {
        InstructionList.Children.Clear();
    }
}