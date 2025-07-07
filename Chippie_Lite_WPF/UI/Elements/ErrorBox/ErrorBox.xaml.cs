using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ErrorBox : UserControl
{
    private Point mousePoint;
    private Canvas? canvas;
    
    
    public ErrorBox()
    {
        InitializeComponent();
        RenderTransform = new TranslateTransform();
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
        Visibility = Visibility.Collapsed;
    }
    
    
    private void DragBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        mousePoint = e.GetPosition(canvas);
        DragBox.CaptureMouse();
    }
    private void DragBox_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (!DragBox.IsMouseCaptured) return;
        
        Point mousePosition = e.GetPosition( canvas );
        Canvas.SetLeft( this, Canvas.GetLeft( this ) + mousePosition.X - mousePoint.X );
        Canvas.SetTop ( this, Canvas.GetTop ( this ) + mousePosition.Y - mousePoint.Y );
        
        DragBox.ReleaseMouseCapture();
    }
    private void DragBox_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (!DragBox.IsMouseCaptured || e.LeftButton != MouseButtonState.Pressed) return;
        
        Point mousePosition = e.GetPosition( canvas );

        // Compute the new Left & Top coordinates of the control
        Canvas.SetLeft( this, Canvas.GetLeft( this ) + mousePosition.X - mousePoint.X );
        Canvas.SetTop ( this, Canvas.GetTop ( this ) + mousePosition.Y - mousePoint.Y );
        mousePoint = mousePosition;
    }
    private void ErrorBox_OnLoaded(object sender, RoutedEventArgs e)
    {
        canvas = Parent as Canvas;
        if ( canvas == null ) {
            throw new InvalidCastException( "The parent of a KeyboardPopup control must be a Canvas." );
        } 
    }
}