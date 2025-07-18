using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class RegisterList : UserControl
{
    private ControlMode mode = ControlMode.View;
    private Brush ParentBorderBrush;


    public ControlMode Mode
    {
        get => mode;
        set
        {
            bool changed = mode != value;
            mode = value;
            if (!changed) return;
            
        }
    }
    
    public RegisterList()
    {
        InitializeComponent();
        FetchColors();
        LoadRegisters();
    }

    private void FetchColors()
    {
        try
        {
            ParentBorderBrush = ((Parent as Border)!).BorderBrush;
        }
        catch (Exception)
        {
            ParentBorderBrush = (Application.Current.Resources["Deep Purple"] as Brush)!;
        }
    }
    
    private void LoadRegisters()
    {
        foreach (var register in RegisterBank.GetAllRegisters())
        {
            AddRegisterItem(register);
        }
    }

    private void AddRegisterItem(Register register)
    {
        Border itemBorder = new Border()
        {
            BorderThickness = new Thickness(0, 0, 0, 4),
            BorderBrush = ParentBorderBrush
        };
        RegisterListItem item = new RegisterListItem(register, Mode);
        itemBorder.Child = item;
        RegisterListPanel.Children.Add(itemBorder);
    }
}