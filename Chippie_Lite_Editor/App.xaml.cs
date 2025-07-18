using System.Windows;
using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        Chippie.Initialize();
        Memory.Write(1, 10);
    }
}