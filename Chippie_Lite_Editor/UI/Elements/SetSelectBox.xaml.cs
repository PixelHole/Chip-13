using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class SetSelectBox
{
    public SetSelectBox()
    {
        InitializeComponent();
        ConnectEvents();
        UpdateSetComboBox();
    }
    private void ConnectEvents()
    {
        InstructionSetManager.OnSetLoaded += (_, _) => UpdateSetComboBox();
        InstructionSetManager.OnSetUnloaded += (_, _) => UpdateSetComboBox();
        InstructionSetManager.OnActiveSetChanged += (_, index) => InstructionSetCB.SelectedIndex = index;
    }
    private void InstructionSetSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = (e.AddedItems[0] as ComboBoxItem)!.Content.ToString() ?? "";
        InstructionSetManager.SetActive(item);
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
            if (i < InstructionSetCB.Items.Count) (InstructionSetCB.Items[i] as ComboBoxItem)!.Content = names[i];
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
}