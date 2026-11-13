using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements.Config_View_Elements.RegisterEditBox;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Elements;

public class RegisterListControl
{
    private RegisterEditWindow? CreateWindow = null;
    private RegisterList View;


    public RegisterListControl(RegisterList view)
    {
        View = view;
    }

    public void ShowCreateWindow()
    {
        if (CreateWindow != null)
        {
            FocusOnCreateWindow();
            WPFUtility.FlashWindow(CreateWindow);
            WPFUtility.BringWindowToTop(CreateWindow);
            return;
        }
        
        CreateWindow = new RegisterEditWindow(new Register("", "")) {
            Owner = MainWindow.MainWindowIns
        };
        CreateWindow.Closed += CreateWindowOnClosed;
        CreateWindow.OnEditOk += CreateWindowOnOnEditOk;
        CreateWindow.Show();
    }
    private void CreateWindowOnOnEditOk(object sender, Register register)
    {
        if (RegisterBank.AddRegister(register)) return;
        ErrorBox.Pop("Could not add register",
            $"could not add register because a register with name {register.Name} or " +
            $"the id {register.Id} already exists in the register bank.");
    }
    private void CreateWindowOnClosed(object? sender, EventArgs e)
    {
        CreateWindow = null;
    }

    private void FocusOnCreateWindow()
    {
        CenterCreateWindow();
    }
    private void CenterCreateWindow()
    {
        if (CreateWindow != null) WPFUtility.CenterWindow(CreateWindow);
    }
}