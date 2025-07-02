using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ErrorBox : UserControl
{
    public ErrorBox()
    {
        InitializeComponent();
    }

    public void Pop(double x, double y, string title = "Error", string msg = "An error has occured")
    {
        SetPosition(x, y);
        SetMessage(title, msg);
        Show();
    }

    public void SetPosition(double x, double y)
    {
        Canvas.SetLeft(this, x);
        Canvas.SetTop(this, y);
    }
    public void SetMessage(string title = "Error", string msg = "An error has occured")
    {
        TitleLabel.Content = title;
        MessageBox.Text = msg;
    }
    public void Show()
    {
        Visibility = Visibility.Visible;
    }
    
    private void CloseBtn_OnClick(SquareButton sender)
    {
        Visibility = Visibility.Hidden;
    }
    
    private void DragBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        
    }
}