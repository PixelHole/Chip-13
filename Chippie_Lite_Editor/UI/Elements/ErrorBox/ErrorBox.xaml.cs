using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ErrorBox
{
    private Point mousePoint;
    private Canvas? canvas;
    
    
    private ErrorBox()
    {
        InitializeComponent();
        SizeToContent = SizeToContent.WidthAndHeight;
    }

    public static ErrorBox Pop(string title = "Error", string msg = "An error has occured", Point point = default)
    {
        ErrorBox box = new ErrorBox();
        box.SetMessage(title, msg);
        
        box.Show();
        
        box.SetPosition(point);

        return box;
    }

    public void SetPosition(Point point)
    {
        Left = point.X;
        Top = point.Y;
    }
    public void SetMessage(string title = "Error", string msg = "An error has occured")
    {
        TitleLabel.Content = title;
        MessageBox.Text = msg;
    }
    
    private void CloseBtn_OnClick(SquareButton sender)
    {
        Close();
    }
    
    
    private void DragBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}