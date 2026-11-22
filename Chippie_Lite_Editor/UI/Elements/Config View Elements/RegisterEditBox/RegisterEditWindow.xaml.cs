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
    
    
    public RegisterEditWindow(Register assignedRegister, string title)
    {
        InitializeComponent();
        AssignedRegister = assignedRegister;
        ShowInTaskbar = false;
        Frame.Title = title;
    }

    private void LoadRegisterData()
    {
        NameBox.Text = AssignedRegister.Name;
        IdBox.Text = AssignedRegister.Id;
        ValueBox.Text = AssignedRegister.DefaultValue.ToString();
        LengthLimitBox.Text = AssignedRegister.LengthLimit.ToString();
    }
    private bool FinalizeEdit()
    {
        if (string.IsNullOrEmpty(NameBox.Text))
        {
            ErrorBox.Pop("the register must have a name!", "the register name cannot be empty");
            return false;
        }
        if (string.IsNullOrEmpty(IdBox.Text))
        {
            ErrorBox.Pop("the register must have an Id!", "the register id cannot be empty");
            return false;
        }
        if (string.IsNullOrEmpty(ValueBox.Text))
        {
            ErrorBox.Pop("the register must have a value!", "the registers value cannot be empty");
            return false;
        }
        if (string.IsNullOrEmpty(LengthLimitBox.Text))
        {
            ErrorBox.Pop("the register must have a length limit!", "the registers Length Limit cannot be empty");
            return false;
        }
        
        string name = NameBox.Text,
            id = IdBox.Text,
            val = ValueBox.Text,
            length = LengthLimitBox.Text;

        AssignedRegister.Name = name;
        AssignedRegister.Id = id;
        AssignedRegister.DefaultValue = NumberUtility.ParseNumber(val);
        AssignedRegister.LengthLimit = (byte)NumberUtility.ParseNumber(length);
        
        OnEditOk?.Invoke(this, AssignedRegister);
        return true;
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
        if (FinalizeEdit()) Close();
    }
    private void CancelBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Close();
    }
}