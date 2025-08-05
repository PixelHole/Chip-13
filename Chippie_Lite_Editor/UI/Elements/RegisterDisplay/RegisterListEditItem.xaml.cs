using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class RegisterListEditItem
{
    private Register assignedRegister = null!;
    
    
    public RegisterListEditItem(Register register)
    {
        InitializeComponent();
        AssignRegister(register);
        LoadRegisterData();
        ConnectEvents();
    }
    private void AssignRegister(Register register)
    {
        assignedRegister = register;
    }
    private void LoadRegisterData()
    {
        IdLabel.Content = assignedRegister.Id;
        ContentBox.SetText(assignedRegister.DefaultValue.ToString());
    }
    private void ConnectEvents()
    {
        ContentBox.TextConfirmed += text => assignedRegister.DefaultValue = NumberUtility.ParseNumber(text);
    }

    private bool TextBoxCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out _);
    }
}