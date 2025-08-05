using System.Windows;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File;
using Chippie_Lite_WPF.Documentation;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        try
        {
            Chippie.Initialize();
        }
        catch (Exception exception)
        {
            ChippeExceptionHandler.HandleInitException(exception);
        }
    }
}