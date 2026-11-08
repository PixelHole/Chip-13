using System.Windows;
using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Cat;

public partial class CatContainer : UserControl
{
    public CatContainerControl Control { get; }
    public double MaxWidth { get; private set; }
    
    public CatContainer()
    {
        InitializeComponent();
        ConnectEvents();
        Control = new CatContainerControl(this);
    }

    private void ConnectEvents()
    {
        Application.Current.Exit += CurrentOnExit;
    }
    private void CurrentOnExit(object sender, ExitEventArgs e)
    {
        Control.DespawnAllCats();
    }


    private void CatContainer_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        MaxWidth = e.NewSize.Width;
    }
}