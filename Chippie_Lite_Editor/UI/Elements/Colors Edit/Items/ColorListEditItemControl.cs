using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Elements;

public class ColorListEditItemControl
{
    private ColorListEditItem View { get; set; }
    private static ColorPickerWindow? colorPickerWindow;


    public ColorListEditItemControl(ColorListEditItem view)
    {
        View = view;
    }

    public void OpenColorWindow()
    {
        if (colorPickerWindow != null)
        {
            WPFUtility.CenterWindow(colorPickerWindow);
            colorPickerWindow.SetColor(View.AssignedColor);
            return;
        }
        
        colorPickerWindow = new ColorPickerWindow(View.AssignedColor)
        {
            Owner = MainWindow.MainWindowIns
        };
        colorPickerWindow.Closed += (_, _) => colorPickerWindow = null;
        colorPickerWindow.OnConfirm += ColorPickerWindowOnOnConfirm;
        colorPickerWindow.Show();
    }
    private void ColorPickerWindowOnOnConfirm(object sender, Color color)
    {
        ColorLibrary.SetColor(color, View.Index);
    }
}