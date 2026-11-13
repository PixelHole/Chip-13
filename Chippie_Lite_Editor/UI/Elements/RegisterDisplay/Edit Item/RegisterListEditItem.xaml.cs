using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public sealed partial class RegisterListEditItem
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(RegisterListEditItem),
        new PropertyMetadata(false)
    );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    internal RegisterList List;
    private RegisterListEditControl Control { get; }

    public delegate void RegisterEditAction(object sender, Register updated);
    public event RegisterEditAction? OnRegisterEdited;
    
    
    public RegisterListEditItem(RegisterList list)
    {
        List = list;
        Control = new RegisterListEditControl(this);
        InitializeComponent();
        ConnectEvents();
    }
    protected override void LoadRegisterData()
    {
        WPFUtility.SetContent(NameLabel, AssignedRegister?.Name ?? "Name");
        WPFUtility.SetContent(IdLabel, AssignedRegister?.Id ?? "Id");
        WPFUtility.SetContent(SizeLabel, $"[{AssignedRegister?.LengthLimit.ToString() ?? "Length"}]");
    }
    private void ConnectEvents()
    {
        
    }

    private void DeleteRegister()
    {
        Control.DeleteRegister();
    }
    private void EditRegister()
    {
        Control.EditRegister();
    }
    private void DuplicateRegister()
    {
        Control.DuplicateRegister();
    }

    public void RegisterEditedCall()
    {
        LoadRegisterData();
    }
    
    
    private void EditBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        EditRegister();
    }
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        DuplicateRegister();
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        DeleteRegister();
    }
    
    private void RegisterListEditItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        EditRegister();
    }
    
    private void RegisterListEditItem_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void RegisterListEditItem_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;
    }
    private void RegisterListEditItem_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
}