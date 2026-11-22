using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements.Config_View_Elements.RegisterEditBox;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Elements;

public class RegisterListEditControl
{
    private static RegisterEditWindow? EditWindow { get; set; }
    private RegisterListEditItem View { get; set; }


    public RegisterListEditControl(RegisterListEditItem view)
    {
        View = view;
    }

    public void EditRegister()
    {
        if (EditWindow != null)
        {
            FocusOnEditWindow();
            WPFUtility.FlashWindow(EditWindow);
            WPFUtility.BringWindowToTop(EditWindow);
            EditWindow.AssignedRegister = View.AssignedRegister;
            return;
        }

        
        EditWindow = new RegisterEditWindow(View.AssignedRegister!, "Edit Register") {
            Owner = MainWindow.MainWindowIns
        };
        EditWindow.Closed += EditWindowOnClosed;
        EditWindow.OnEditOk += EditWindowOnOnEditOk;
        EditWindow.Show();
    }
    private void EditWindowOnOnEditOk(object sender, Register register)
    {
        View.RegisterEditedCall();
    }
    private void EditWindowOnClosed(object? sender, EventArgs e)
    {
        EditWindow = null;
    }
    public void DuplicateRegister()
    {
        var assigned = View.AssignedRegister;
        if (assigned == null) return;
        int index = RegisterBank.IndexOf(assigned);
        Register register = new Register(assigned.Name + "Copy", assigned.Id + 'D', assigned.Content, assigned.DefaultValue, assigned.LengthLimit);
        if (RegisterBank.InsertRegister(register, index + 1)) return;
        ErrorBox.Pop("Could not duplicate register",
            $"could not add register because a register with name {register.Name} or " +
            $"the id {register.Id} already exists in the register bank.");
    }
    public void DeleteRegister()
    {
        ConfirmationPrompt deletePrompt = new ConfirmationPrompt("Are you sure?", "Do you want to permanently delete this register?" +
            " this will remove the register from Chip-13's Register Bank", ["Yes", "No"],
            s =>
            {
                if (s == "Yes" && View.AssignedRegister != null) RegisterBank.RemoveRegister(View.AssignedRegister);
            });
        deletePrompt.Show();
    }

    private void FocusOnEditWindow()
    {
        CenterEditWindow();
    }
    private void CenterEditWindow()
    {
        if (EditWindow != null) WPFUtility.CenterWindow(EditWindow);
    }
}