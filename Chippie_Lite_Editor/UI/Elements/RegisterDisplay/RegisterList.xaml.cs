using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements.Config_View_Elements.RegisterEditBox;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class RegisterList : UserControl
{
    private ControlMode mode = ControlMode.View;
    private RegisterListControl Control { get; }

    public required ControlMode Mode
    {
        get => mode;
        set
        {
            bool changed = mode != value;
            mode = value;
            if (!changed) return;
            UpdateList();
        }
    }
    
    
    public RegisterList()
    {
        InitializeComponent();
        ConnectEvents();
        Control = new RegisterListControl(this);
    }
    
    private void ConnectEvents()
    {
        RegisterBank.OnRegisterAdded += RegisterBankOnOnRegisterAdded;
        RegisterBank.OnRegisterRemoved += RegisterBankOnOnRegisterRemoved;
    }
    
    private void UpdateList()
    {
        UpdateListRows();
        UpdateItems();
    }
    
    private void UpdateItems()
    {
        if (RegisterBank.Count == 0)
        {
            RegisterListGrid.Children.Clear();
            return;
        }

        for (int i = 0; i < RegisterListGrid.Children.Count; i++)
        {
            var item = RegisterListGrid.Children[i] as RegisterListItem;
            item!.AssignedRegister = RegisterBank.GetRegister(i);
            Grid.SetRow(item, i);
        }

        for (int i = RegisterListGrid.Children.Count; i > RegisterBank.Count; i--) RemoveItem(i - 1);
        
        for (int i = RegisterListGrid.Children.Count; i < RegisterBank.Count; i++)
        {
            var item = AddItem();
            Grid.SetRow(item, i);
            item.AssignedRegister = RegisterBank.GetRegister(i);
        }
    }
    private RegisterListItem AddItem()
    {
        RegisterListItem item = null!;
        switch (Mode)
        {
            case ControlMode.View :
                item = new RegisterListDisplayItem();
                break;
            
            case ControlMode.Edit :
                item = new RegisterListEditItem(this);
                break;
        }
        RegisterListGrid.Children.Add(item);
        return item;
    }
    private void RemoveItem(int index)
    {
        RegisterListGrid.Children.RemoveAt(index);
    }

    private void ClearListRows()
    {
        WPFUtility.DoUiAction(RegisterListGrid, () => RegisterListGrid.RowDefinitions.Clear());
    }
    private void UpdateListRows()
    {
        int count = RegisterBank.Count;
        if (count == 0)
        {
            RegisterListGrid.RowDefinitions.Clear();
            return;
        }
        if (RegisterListGrid.RowDefinitions.Count == count) return;

        int diff = int.Abs(RegisterListGrid.RowDefinitions.Count - count);

        for (int i = 0; i < diff; i++)
        {
            if (RegisterListGrid.RowDefinitions.Count < count) AddListRow();
            else RemoveListRow();
        }
    }
    private void AddListRow()
    {
        WPFUtility.DoUiAction(RegisterListGrid, () => RegisterListGrid.RowDefinitions.Add(new RowDefinition()));
    }
    private void RemoveListRow()
    {
        WPFUtility.DoUiAction(RegisterListGrid, () => RegisterListGrid.RowDefinitions.RemoveAt(RegisterListGrid.RowDefinitions.Count - 1));
    }
    
    private void RegisterBankOnOnRegisterRemoved(Register register, int index)
    {
        UpdateList();
    }
    private void RegisterBankOnOnRegisterAdded(Register register, int index)
    {
        UpdateList();
    }
    private void AddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Control.ShowCreateWindow();
    }
}