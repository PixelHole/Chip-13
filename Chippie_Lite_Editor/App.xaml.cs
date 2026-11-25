using System.Windows;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Handlers;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF;


public partial class App
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
    private void App_OnExit(object sender, ExitEventArgs e)
    {
        Chippie.ShutdownProcedure();
    }
}