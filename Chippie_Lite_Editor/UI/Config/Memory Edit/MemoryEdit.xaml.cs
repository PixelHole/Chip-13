using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config;

public partial class MemoryEdit : UserControl
{
    public MemoryEdit()
    {
        InitializeComponent();
        RefreshItems();
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
    
    private void AddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        
    }
}