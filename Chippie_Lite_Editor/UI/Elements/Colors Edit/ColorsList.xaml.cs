using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ColorsList : UserControl
{
    private ControlMode mode = ControlMode.View;

    public ControlMode Mode
    {
        get => mode;
        set
        {
            if (mode == value) return;
            mode = value;
            RefreshItems();
        }
    }

    private ColorsListControl Control { get; set; }
    
    public ColorsList()
    {
        InitializeComponent();
        ConnectEvents();
        Control = new ColorsListControl(this);
    }
    private void ConnectEvents()
    {
        ColorLibrary.OnColorAdded += ColorLibraryOnOnColorAdded;
        ColorLibrary.OnColorRemoved += ColorLibraryOnOnColorRemoved;
        ColorLibrary.OnColorUpdated += ColorLibraryOnOnColorUpdated;
    }

    private void RefreshItems()
    {
        ClearItems();
        UpdateItems();
    }
    private void ClearItems() => ItemPanel.Children.Clear();
    private void UpdateItems()
    {
        if (ItemPanel.Children.Count > ColorLibrary.Count) 
            while (ItemPanel.Children.Count > ColorLibrary.Count) RemoveItem(ItemPanel.Children.Count - 1);
        else if (ItemPanel.Children.Count < ColorLibrary.Count) 
            while (ItemPanel.Children.Count < ColorLibrary.Count) AddItem();
        
        for (int i = 0; i < ItemPanel.Children.Count; i++)
        {
            var item = (ItemPanel.Children[i] as ColorsListItem)!;
            item.AssignedBrush = new SolidColorBrush(ColorLibrary.GetColor(i));
            item.Index = i;
        }
    }
    private ColorsListItem AddItem()
    {
        ColorsListItem item;
        if (Mode == ControlMode.Edit) item = new ColorListEditItem();
        else item = new ColorListDisplayItem();
        WPFUtility.DoUiAction(ItemPanel, () => ItemPanel.Children.Add(item));
        return item;
    }
    private void RemoveItem(int index)
    {
        WPFUtility.DoUiAction(ItemPanel, () => ItemPanel.Children.RemoveAt(index));
    }
    
    private void ColorLibraryOnOnColorAdded(Color color, int index)
    {
        UpdateItems();
    }
    private void ColorLibraryOnOnColorRemoved(Color color, int index)
    {
        RemoveItem(index);
    }
    private void ColorLibraryOnOnColorUpdated(Color color, int index)
    {
        UpdateItems();
    }
    private void AddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Control.ShowCreateWindow();
    }
}