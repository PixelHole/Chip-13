using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Documentation;

namespace Chippie_Lite_WPF.UI.Windows.Help;

public partial class DocumentListItem : UserControl
{
    private string text = "";
    private bool collapsed = true;
    private bool mouseInside;
    private bool mouseDown;
    private ContentListItemMode mode;

    private HelpWindow Owner;
    
    public ContentListItemMode Mode
    {
        get => mode;
        set
        {
            if (mode == value) return;
            mode = value;
            UpdateMode();
        }
    }

    private Brush BackgroundDefaultColor { get; set; } = null!;
    private Brush BackgroundHoverColor { get; set; } = null!;
    private Brush BackgroundClickColor { get; set; } = null!;

    private Brush CollapsedTextColor { get; set; } = null!;
    private Brush OpenTextColor { get; set; } = null!;

    private new Brush Background {
        get
        {
            Brush bg = null!;
            if (Body.CheckAccess())
            {
                bg = Body.Background;
            }
            else
            {
                Body.Dispatcher.Invoke(() => bg = Body.Background);
            }

            return bg;
        }
        set
        {
            if (Body.CheckAccess())
            {
                Body.Background = value;
            }
            else
            {
                Body.Dispatcher.Invoke(() => Body.Background = value);
            }
        } 
    }

    public string Text
    {
        get => text;
        set
        {
            if (text == value) return;
            text = value;
            if (TitleBlock.CheckAccess())
            {
                TitleBlock.Text = text;
            }
            else
            {
                TitleBlock.Dispatcher.Invoke(() => TitleBlock.Text = text);
            }
        }
    }

    public bool Collapsed
    {
        get => collapsed;
        set
        {
            if (collapsed == value) return;
            collapsed = value;
            if (CheckAccess()) UiUpdateCollapsed();
            else Dispatcher.Invoke(UiUpdateCollapsed);
        }
    }

    private void UiUpdateCollapsed()
    {
        if (Mode == ContentListItemMode.Directory)
        {
            TitleBlock.Foreground = collapsed ? CollapsedTextColor : OpenTextColor;
            ExpandIcon.Foreground = collapsed ? CollapsedTextColor : OpenTextColor;
            ExpandIcon.Text = collapsed ? "+" : "-";
        }
        foreach (UIElement child in SubdirectoriesPanel.Children)
        {
            if (collapsed && child is DocumentListItem item) item.Collapsed = collapsed;
            child.Visibility = collapsed ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    private DocumentTreeNode ConnectedNode { get; }
    
    
    public DocumentListItem(HelpWindow owner, DocumentTreeNode connectedNode)
    {
        InitializeComponent();
        FetchColors();
        Owner = owner;
        ConnectedNode = connectedNode;
        Text = ConnectedNode.Name;
        Mode = ConnectedNode.Document == null ? ContentListItemMode.Directory : ContentListItemMode.Content;
        CreateSubDirectories();
        UiUpdateCollapsed();
    }
    private void FetchColors()
    {
        BackgroundDefaultColor = new SolidColorBrush(Colors.Transparent);
        BackgroundHoverColor = (Application.Current.Resources["Deep Purple"] as Brush)!;
        BackgroundClickColor = (Application.Current.Resources["Red"] as Brush)!;
        CollapsedTextColor = (Application.Current.Resources["Light Grey"] as Brush)!;
        OpenTextColor = (Application.Current.Resources["Pink"] as Brush)!;
    }

    private void CreateSubDirectories()
    {
        foreach (var subNode in ConnectedNode.Children)
        {
            DocumentListItem sub = new DocumentListItem(Owner, subNode)
            {
                Visibility = Collapsed ? Visibility.Collapsed : Visibility.Visible
            };
            if (CheckAccess())
            {
                SubdirectoriesPanel.Children.Add(sub);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    SubdirectoriesPanel.Children.Add(sub);
                });
            }
        }
    }
    
    private void UpdateColor()
    {
        if (mouseDown) Background = BackgroundClickColor;
        else Background = mouseInside ? BackgroundHoverColor : BackgroundDefaultColor;
    }
    
    private void UpdateMode()
    {
        if (Mode == ContentListItemMode.Directory) DirectoryMode();
        else ContentMode();
    }
    private void DirectoryMode()
    {
        if (CheckAccess())
        {
            Icon.Source = Application.Current.Resources["FolderIcon"] as ImageSource;
        }
        else
        {
            Dispatcher.Invoke(() =>
            {
                Icon.Source = Application.Current.Resources["FolderIcon"] as ImageSource;
            });
        }
    }
    private void ContentMode()
    {
        if (CheckAccess())
        {
            ExpandIcon.Text = ">";
            Icon.Source = Application.Current.Resources["FileIcon"] as ImageSource;
        }
        else
        {
            Dispatcher.Invoke(() =>
            {
                ExpandIcon.Text = ">";
                Icon.Source = Application.Current.Resources["FileIcon"] as ImageSource;
            });
        }
    }

    private void ClickHandler(MouseButtonEventArgs e)
    {
        switch (e.ClickCount)
        {
            case 1:
                OnClicked();
                break;
            case > 1:
                OnDoubleClicked();
                break;
        }

        e.Handled = true;
    }
    private void OnClicked()
    {
        if (Mode == ContentListItemMode.Directory) DirectoryModeOnClick();
    }
    private void DirectoryModeOnClick()
    {
        Collapsed = !Collapsed;
    }
    private void ContentModeOnClick()
    {
        
    }
    
    private void OnDoubleClicked()
    {
        if (Mode == ContentListItemMode.Directory) DirectoryModeDoubleClick();
        else ContentModeOnDoubleClick();
    }
    private void DirectoryModeDoubleClick()
    {
        
    }
    private void ContentModeOnDoubleClick()
    {
        if (ConnectedNode.Document == null) return;
        Owner.OpenTabRequest(ConnectedNode.Document);
    }
    
    private void Body_OnMouseEnter(object sender, MouseEventArgs e)
    {
        mouseInside = true;
        UpdateColor();
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        mouseInside = false;
        mouseDown = false;
        UpdateColor();
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        mouseDown = true;
        mouseInside = true;
        UpdateColor();
        ClickHandler(e);
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        mouseDown = false;
        UpdateColor();
    }
}