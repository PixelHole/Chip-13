using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class RunWindow : UserControl
{
    public RunWindow()
    {
        InitializeComponent();
    }

    public void LoadDataForRun()
    {
        
    }

    private void ToggleRegistersBtn_OnOnSelected(ToggleButton sender, bool selected)
    {
        ColumnGrid.ColumnDefinitions[2].Width = selected ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
    }
    private void ToggleMemoryBtn_OnOnSelected(ToggleButton sender, bool selected)
    {
        RowGrid.RowDefinitions[1].Height = selected ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
    }
    private void ToggleInstructionBtn_OnOnSelected(ToggleButton sender, bool selected)
    {
        ColumnGrid.ColumnDefinitions[0].Width = selected ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
    }
}