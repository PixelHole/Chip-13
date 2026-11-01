using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Cat;

public partial class CatContainer : UserControl
{
    public CatContainerControl Control { get; }
    
    
    public CatContainer()
    {
        Control = new CatContainerControl(this);
        InitializeComponent();
    }
}