using System.Windows.Controls;
using System.Windows.Documents;

namespace Chippie_Lite_WPF.UI.Help;

public partial class FormattedTextDisplay : UserControl
{
    public string Text
    {
        set
        {
            if (TextBox.CheckAccess()) UpdateDisplayText(value);
            else TextBox.Dispatcher.Invoke(() => UpdateDisplayText(value));
        }
    }
    
    public FormattedTextDisplay()
    {
        InitializeComponent();
    }

    private void UpdateDisplayText(string text)
    {
        
    }
}