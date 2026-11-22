using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config;

public partial class MemoryEdit
{
    private Memory? EditMemory;
    private bool create = false;
    
    public MemoryEdit()
    {
        InitializeComponent();
        RefreshItems();
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        ChippieComponentManager.OnComponentAdded += ComponentManagerOnComponentAdded;
        ChippieComponentManager.OnComponentRemoved += ComponentManagerOnComponentRemoved;
        
        PropertiesEdit.OnConfirmRequest += PropertiesEditOnOnConfirmRequest;
        PropertiesEdit.OnCancelRequest += PropertiesEditOnOnCancelRequest;
    }

    private void EditModule(Memory memory)
    {
        EditMemory = memory;
        MemoryList.Visibility = Visibility.Collapsed;
        PropertiesEdit.Visibility = Visibility.Visible;
        PropertiesEdit.AssignedMemory = memory.Clone();
    }
    
    private void RefreshItems()
    {
        ClearItems();
        UpdateItems();
    }
    private void ClearItems() => MemoryPanel.Children.Clear();
    private void UpdateItems()
    {
        var modules = ChippieComponentManager.GetComponentsOfType<Memory>();

        if (modules.Count == 0)
        {
            ClearItems();
            return;
        }
        
        if (MemoryPanel.Children.Count > ColorLibrary.Count) 
            while (MemoryPanel.Children.Count > modules.Count) RemoveItem(MemoryPanel.Children.Count - 1);
        else
            while (MemoryPanel.Children.Count < modules.Count) AddItem();
        
        for (int i = 0; i < MemoryPanel.Children.Count; i++)
        {
            var item = (MemoryPanel.Children[i] as MemoryEditItem)!;
            item.AssignedMemory = modules[i];
            item.LoadMemoryData();
            item.OnDeleteRequest += ItemOnDeleteRequest;
            item.OnDuplicateRequest += ItemOnDuplicateRequest;
            item.OnEditRequest += ItemOnEditRequest;
        }
    }
    private void AddItem()
    {
        MemoryEditItem item = new MemoryEditItem();
        WPFUtility.DoUiAction(MemoryPanel, () => MemoryPanel.Children.Add(item));
    }
    private void RemoveItem(int index)
    {
        WPFUtility.DoUiAction(MemoryPanel, () => MemoryPanel.Children.RemoveAt(index));
    }
    
    private void ItemOnEditRequest(MemoryEditItem sender, Memory memory)
    {
        EditModule(memory);
    }
    private void ItemOnDuplicateRequest(MemoryEditItem sender, Memory memory)
    {
        ChippieComponentManager.AddComponent(memory.Clone());
    }
    private void ItemOnDeleteRequest(MemoryEditItem sender, Memory memory)
    {
        ConfirmationPrompt prompt = new ConfirmationPrompt("Are you sure?", "doing this will delete the memory module and all its data.",
            ["Delete", "Cancel"],
            s => { if (s == "Delete") ChippieComponentManager.RemoveComponent(memory); });
        prompt.Show();
    }
    
    private void PropertiesEditOnOnConfirmRequest(MemoryProperties sender, Memory memory)
    {
        if (EditMemory == null)
        {
            if (!create) return;
            ChippieComponentManager.AddComponent(memory);
            EditMemory = memory;
            create = false;
        }
        EditMemory.Name = memory.Name;
        EditMemory.Size = memory.Size;
        EditMemory.LengthLimit = memory.LengthLimit;
        EditMemory.ReadOnly = memory.ReadOnly;
        EditMemory.Permanent = memory.Permanent;
        EditMemory.LoadInitialBlocks(memory.GetAllInitialBlocks());

        MemoryList.Visibility = Visibility.Visible;
        PropertiesEdit.Visibility = Visibility.Collapsed;
        
        UpdateItems();
    }
    private void PropertiesEditOnOnCancelRequest(MemoryProperties sender, Memory memory)
    {
        MemoryList.Visibility = Visibility.Visible;
        PropertiesEdit.Visibility = Visibility.Collapsed;
    }
    
    private void ComponentManagerOnComponentRemoved(ChippieComponent component, int index)
    {
        UpdateItems();
    }
    private void ComponentManagerOnComponentAdded(ChippieComponent component, int index)
    {
        UpdateItems();
    }
    
    private void AddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Memory memory = new Memory("New Memory");
        create = true;
        EditModule(memory);
    }
}