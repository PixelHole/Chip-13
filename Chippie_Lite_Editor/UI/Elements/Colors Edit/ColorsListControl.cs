using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements.Config_View_Elements.RegisterEditBox;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Elements;

public class ColorsListControl
{
    private ColorsList View { get; set; }
    public ColorPickerWindow? CreateWindow { get; set; } 

    public ColorsListControl(ColorsList view)
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
        
        CreateWindow = new ColorPickerWindow(Colors.Azure) {
            Title = "Create a color",
            Owner = MainWindow.MainWindowIns
        };
        CreateWindow.Closed += CreateWindowOnClosed;
        CreateWindow.OnConfirm += CreateWindowOnOnConfirm;
        CreateWindow.Show();
    }
    private void CreateWindowOnOnConfirm(object sender, Color color)
    {
        ColorLibrary.AddColor(color);
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