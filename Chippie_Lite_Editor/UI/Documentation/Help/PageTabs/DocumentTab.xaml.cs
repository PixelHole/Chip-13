using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Documentation;
using Chippie_Lite_WPF.UI.Elements;
using Chippie_Lite_WPF.UI.Windows.Help;

namespace Chippie_Lite_WPF.UI.Help;

public partial class DocumentTab : UserControl
{
    private bool selected;
    private bool mouseInside;
    private bool mouseDown;

    private HelpWindow Owner { get; }
    internal DiskDocument Document { get; }
    
    private Brush BackgroundDefaultColor { get; set; } = null!;
    private Brush BackgroundHoverColor { get; set; } = null!;
    private Brush BackgroundClickColor { get; set; } = null!;
    
    public bool Selected
    {
        get => selected;
        set
        {
            if (selected == value) return;
            selected = value;
            if (CheckAccess())
            {
                HighlightUnderline.Visibility = Selected ? Visibility.Visible : Visibility.Hidden;
                UpdateColor();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    HighlightUnderline.Visibility = Selected ? Visibility.Visible : Visibility.Hidden;
                    UpdateColor();
                });
            }
        }
    }

    private string Title
    {
        set
        {
            if (TitleBlock.CheckAccess()) TitleBlock.Text = value;
            else TitleBlock.Dispatcher.Invoke(() => TitleBlock.Text = value);
        }
    }
    
    
    public DocumentTab(DiskDocument document, HelpWindow owner)
    {
        Owner = owner;
        Document = document;
        InitializeComponent();
        FetchColors();
        Title = document.Name;
    }
    private void FetchColors()
    {
        BackgroundDefaultColor = new SolidColorBrush(Colors.Transparent);
        BackgroundHoverColor = (Application.Current.Resources["Deep Purple"] as Brush)!;
        BackgroundClickColor = (Application.Current.Resources["Red"] as Brush)!;
    }

    private void UpdateColor()
    {
        if (mouseDown) Body.Background = BackgroundClickColor;
        else Body.Background = mouseInside ? BackgroundHoverColor : BackgroundDefaultColor;
    }
    
    private void CloseBtn_OnClick(SquareButton sender)
    {
        Owner.CloseTabRequest(this);
    }

    public bool MatchDocument(DiskDocument doc) => Document == doc;
    
    private void Body_OnMouseEnter(object sender, MouseEventArgs e)
    {
        mouseInside = true;
        UpdateColor();
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        mouseDown = false;
        mouseInside = false;
        UpdateColor();
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        mouseDown = true;
        mouseInside = true;
        UpdateColor();
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        mouseDown = false;
        Owner.SelectTabRequest(this);
        UpdateColor();
    }
}