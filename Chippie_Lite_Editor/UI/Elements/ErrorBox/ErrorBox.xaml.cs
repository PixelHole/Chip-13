using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ErrorBox
{ 
    private ErrorBox()
    {
        InitializeComponent();
        Application.Current.Exit += (sender, args) => Close();
        SizeToContent = SizeToContent.WidthAndHeight;
    }

    public static ErrorBox Pop(string title = "Error", string msg = "An error has occured", Point point = default)
    {
        ErrorBox box = new ErrorBox
        {
            Owner = MainWindow.MainWindowIns
        };
        box.SetMessage(title, msg);
        
        box.Show();
        
        box.SetPosition(point);

        return box;
    }

    public void SetPosition(Point point)
    {
        WPFUtility.DoUiAction(this, () =>
        {
            Left = point.X;
            Top = point.Y;
        });
    }
    public void SetMessage(string title = "Error", string msg = "An error has occured")
    {
        WPFUtility.DoUiAction(this, () =>
        {
            Frame.Title = title;
            MessageBox.Text = msg;
        });
    }
    
    private void CloseBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Close();
    }
}