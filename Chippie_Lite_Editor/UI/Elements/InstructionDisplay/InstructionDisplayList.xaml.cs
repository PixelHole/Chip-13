using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.UI.Elements.InstructionDisplay;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class InstructionDisplayList : UserControl
{
    private Register InstructionPointer { get; set; }
    
    
    public InstructionDisplayList()
    {
        InitializeComponent();
        FetchIP();
        ConnectEvents();
    }
    private void FetchIP()
    {
        InstructionPointer = RegisterBank.GetRegister("Instruction Pointer")!;
    }
    private void ConnectEvents()
    {
        InstructionPointer.OnContentChanged += OnIpContentChanged;
    }

    private void OnIpContentChanged(int value)
    {
        Application.Current.Dispatcher.Invoke(() => HighlightListItem(value));
    }
    
    public void SetListItems(IList<Instruction> instructions)
    {
        ClearInstructionList();
        for (int i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];
            AddInstructionToList(instruction, i);
        }
        HighlightListItem(0);
    }
    private void HighlightListItem(int index)
    {
        for (int i = 0; i < InstructionList.Children.Count; i++)
        {
            var child = InstructionList.Children[i];
            if (child is not InstructionDisplayItem displayItem) continue;
            displayItem.SetHighlighted(i == index);
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