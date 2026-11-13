using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.UI.Elements;

public abstract class RegisterListItem : UserControl
{
    private Register? assignedRegister = null;

    public Register? AssignedRegister
    {
        get => assignedRegister;
        set
        {
            if (assignedRegister == value) return;
            assignedRegister = value;
            LoadRegisterData();
        }
    }

    protected abstract void LoadRegisterData();
}