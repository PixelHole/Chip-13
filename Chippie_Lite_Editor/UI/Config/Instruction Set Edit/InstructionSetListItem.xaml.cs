using System.Windows;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.InstructionSets;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;
using UserControl = System.Windows.Controls.UserControl;

namespace Chippie_Lite_WPF.UI.Config;

public partial class InstructionSetListItem : UserControl
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(InstructionSetListItem),
        new PropertyMetadata(false)
    );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    private InstructionSetList List;
    
    private InstructionSet? assignedSet;
    public InstructionSet? AssignedSet
    {
        get => assignedSet;
        set
        {
            assignedSet = value;
            OnSetChanged();
        }
    }
    
    
    public InstructionSetListItem(InstructionSetList list)
    {
        List = list;
        InitializeComponent();
    }

    private void OnSetChanged()
    {
        EditBtn.Visibility = AssignedSet != null && (AssignedSet.ReadOnly || AssignedSet.Name == "Chip-13") ? Visibility.Collapsed : Visibility.Visible;
        DeleteBtn.Visibility = AssignedSet != null && (AssignedSet.ReadOnly || AssignedSet.Name == "Chip-13") ? Visibility.Collapsed : Visibility.Visible;
        WPFUtility.SetContent(NameLabel, AssignedSet?.Name ?? "Instruction set");
        WPFUtility.SetContent(CountLabel, AssignedSet == null ? "0" : AssignedSet.Count + " Instructions");
    }
    
    private void ExportBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (AssignedSet != null) InstructionSetFileManager.ExportToFile(AssignedSet, "");
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (AssignedSet == null) return;
        ConfirmationPrompt prompt = new ConfirmationPrompt("Are you sure?",
            "Are you sure you want to delete this instruction set? there will be no way to recover it. consider exporting it if you might need it later",
            ["Export & Delete", "Cancel", "Delete"],
            DeletePromptResult);
        prompt.Show();
    }
    private void DeletePromptResult(string choice)
    {
        if (choice == "Cancel" || AssignedSet == null) return;
        if (choice == "Delete") InstructionSetManager.UnloadSet(AssignedSet);
        else
        {
            InstructionSetFileManager.ExportToFile(AssignedSet, "");
            InstructionSetManager.UnloadSet(AssignedSet!);
        }
    }
    
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        try
        {
            if (AssignedSet != null) InstructionSetManager.LoadSet(InstructionSetManager.DuplicateSet(AssignedSet));
        }
        catch (Exception exception)
        {
            ErrorBox.Pop("Error!", exception.Message);
        }
    }
    private void EditBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (AssignedSet != null) List.EditSet(AssignedSet);
    }
    private void InstructionSetListItem_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void InstructionSetListItem_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;
    }
    private void InstructionSetListItem_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
}