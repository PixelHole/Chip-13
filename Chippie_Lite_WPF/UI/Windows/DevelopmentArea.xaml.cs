using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Windows;

public partial class DevelopmentArea : UserControl
{
    public DevelopmentArea()
    {
        InitializeComponent();
        ConnectEvents();
    }

    public void ChangePage(int index)
    {
        switch (index)
        {
            case 0 :
                CodeEditControl.Visibility = Visibility.Visible;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
            
            case 1 :
                CodeEditControl.Visibility = Visibility.Collapsed;
                MemoryEditControl.Visibility = Visibility.Visible;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
            
            case 2 :
                CodeEditControl.Visibility = Visibility.Collapsed;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Visible;
                break;
            
            default :
                CodeEditControl.Visibility = Visibility.Visible;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
        }
    }
    
    private void ConnectEvents()
    {
        PageSelect.OnPageChanged += OnPageTabChanged;
    }

    private void OnPageTabChanged(int index)
    {
        ChangePage(index);
    }
}