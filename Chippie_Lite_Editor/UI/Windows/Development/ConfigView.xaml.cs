using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class ConfigView
{
    private int page = 0;
    public int Page
    {
        get => page;
        set
        {
            if (value == page) return;
            var old = page;
            page = value;
            UpdatePropertiesPage(old, page);
        }
    }
    
    
    public ConfigView()
    {
        InitializeComponent();
        // TODO : DEBUG PARAMETER
        ConfigList.SelectedIndex = 2;
    }

    private void UpdatePropertiesPage(int old, int newPage)
    {
        WPFUtility.SetVisibility(ConfigPropertiesContainer.Children[old], Visibility.Collapsed);
        WPFUtility.SetVisibility(ConfigPropertiesContainer.Children[newPage], Visibility.Visible);
    }
    
    private void ConfigList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Page = ConfigList.SelectedIndex;
    }
}