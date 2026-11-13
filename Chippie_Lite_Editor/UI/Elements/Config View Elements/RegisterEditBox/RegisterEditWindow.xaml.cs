using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements.Config_View_Elements.RegisterEditBox;

public partial class RegisterEditWindow : Window
{
    private Register assignedRegister = null!;

    public Register AssignedRegister
    {
        get => assignedRegister;
        set
        {
            if (assignedRegister == value) return;
            assignedRegister = value;
            LoadRegisterData();
        }
    }

    public delegate void EditOkAction(object sender, Register register);
    public event EditOkAction? OnEditOk;
    
    
    public RegisterEditWindow(Register assignedRegister)
    {
        InitializeComponent();
        AssignedRegister = assignedRegister;
        ShowInTaskbar = false;
    }

    private void LoadRegisterData()
    {
        NameBox.Text = AssignedRegister.Name;
        IdBox.Text = AssignedRegister.Id;
        ValueBox.Text = AssignedRegister.DefaultValue.ToString();
        LengthLimitBox.Text = AssignedRegister.LengthLimit.ToString();
    }
    private void FinalizeEdit()
    {
        string name = NameBox.Text,
            id = IdBox.Text,
            val = ValueBox.Text,
            length = LengthLimitBox.Text;

        AssignedRegister.Name = name;
        AssignedRegister.Id = id;
        AssignedRegister.DefaultValue = NumberUtility.ParseNumber(val);
        AssignedRegister.LengthLimit = (byte)NumberUtility.ParseNumber(length);
        
        OnEditOk?.Invoke(this, AssignedRegister);
    }
    
    private bool NumberBoxCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out _);
    }
    private bool NameBoxCondition(string s)
    {
        if (AssignedRegister.Name == s) return true;
        var match = RegisterBank.GetRegister(s);
        return IsNameValid(s) && match == null;
    }
    private bool IdBoxCondition(string s)
    {
        if (AssignedRegister.Id == s) return true;
        var match = RegisterBank.GetRegister(s);
        return IsNameValid(s) && match == null;
    }
    private bool IsNameValid(string s)
    {
        return !string.IsNullOrWhiteSpace(s) && s.Trim().All(c => char.IsAsciiLetter(c) || char.IsWhiteSpace(c));
    }
    
    private void SquareButton_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Close();
    }
    private void OkBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        FinalizeEdit();
        Close();
    }
    private void CancelBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Close();
    }
}