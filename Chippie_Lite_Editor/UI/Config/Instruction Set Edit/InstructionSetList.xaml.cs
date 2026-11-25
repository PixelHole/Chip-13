using System.Windows;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.UI.Elements;
using UserControl = System.Windows.Controls.UserControl;

namespace Chippie_Lite_WPF.UI.Config;

public partial class InstructionSetList : UserControl
{
    private InstructionSet? EditingSet;
    
    public InstructionSetList()
    {
        InitializeComponent();
        ConnectEvents();
        UpdateSetList();
    }
    private void ConnectEvents()
    {
        InstructionSetManager.OnSetLoaded += (_, _) => UpdateSetList();
        InstructionSetManager.OnSetUnloaded += (_, _) => UpdateSetList();
        SetEdit.OnCancel += SetEditOnOnCancel;
        SetEdit.OnConfirm += SetEditOnOnConfirm;
    }
    private void SetEditOnOnConfirm(InstructionSetEdit sender, InstructionSet? assignedSet)
    {
        if (EditingSet != null) InstructionSetManager.UnloadSet(EditingSet);
        if (assignedSet != null) InstructionSetManager.LoadSet(assignedSet);
    }
    private void SetEditOnOnCancel(InstructionSetEdit sender, InstructionSet? assignedset)
    {
        HideSetEdit();
        SetEdit.AssignedSet = null;
        EditingSet = null;
    }

    private void UpdateSetList()
    {
        if (InstructionSetManager.Count == 0)
        {
            SetList.Children.Clear();
            return;
        }
        
        while (SetList.Children.Count != InstructionSetManager.Count)
        {
            if (SetList.Children.Count > InstructionSetManager.Count)
                SetList.Children.RemoveAt(SetList.Children.Count - 1);
            else SetList.Children.Add(new InstructionSetListItem(this));
        }

        for (int i = 0; i < SetList.Children.Count; i++)
        {
            var item = (SetList.Children[i] as InstructionSetListItem)!;
            item.AssignedSet = InstructionSetManager.GetSet(i);
        }
    }

    private void CreateSet()
    {
        EditingSet = null;
        SetEdit.AssignedSet = new InstructionSet(InstructionSetManager.GenerateNewName(), []);
        ShowSetEdit();
    }
    public void EditSet(InstructionSet set)
    {
        EditingSet = new InstructionSet(set.Name, set.GetTemplates());
        SetEdit.AssignedSet = EditingSet;
        ShowSetEdit();
    }

    private void ShowSetEdit()
    {
        SetListContainer.Visibility = Visibility.Collapsed;
        SetEdit.Visibility = Visibility.Visible;
    }
    private void HideSetEdit()
    {
        SetListContainer.Visibility = Visibility.Visible;
        SetEdit.Visibility = Visibility.Collapsed;
    }
    private void AddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        CreateSet();
    }
}