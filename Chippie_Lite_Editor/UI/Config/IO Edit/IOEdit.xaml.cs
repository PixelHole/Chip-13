using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;
using wpf_Console;

namespace Chippie_Lite_WPF.UI.Config;

public partial class IOEdit
{
    public IOEdit()
    {
        InitializeComponent();
        ConnectEvents();
        UpdateTextboxContents(IOInterface.DisplaySize);
    }
    private void ConnectEvents()
    {
        IOInterface.DisplaySizeSet += UpdateTextboxContents;
    }

    private void UpdateTextboxContents(Vector2Int size)
    {
        WidthBox.Text = size.x.ToString();
        HeightBox.Text = size.y.ToString();
    }

    public bool NumberCondition(string s)
    {
        return NumberUtility.TryParseNumber(s, out _);
    }
    
    private void HeightBox_OnTextConfirmed(string text)
    {
        int width = (int)NumberUtility.ParseNumber(text);
        IOInterface.DisplaySize.x = width;
    }
    private void WidthBox_OnTextConfirmed(string text)
    {
        int height = (int)NumberUtility.ParseNumber(text);
        IOInterface.DisplaySize.y = height;
    }
}