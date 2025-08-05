using System.Windows;
using System.Windows.Input;
using Chippie_Lite_WPF.Documentation;
using Chippie_Lite_WPF.UI.Help;

namespace Chippie_Lite_WPF.UI.Windows.Help;

public partial class HelpWindow : Window
{
    private DocumentTab? _selectedTab;
    private double bodyFontSize = 24;
    
    private List<DocumentTab> OpenTabs { get; } = [];

    private DocumentTab? SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (_selectedTab == value) return;
            if (_selectedTab != null) _selectedTab.Selected = false;
            _selectedTab = value;
            if (_selectedTab == null) SetDisplayContent("", "");
            else
            {
                _selectedTab.Selected = true;
                SetDisplayContent(_selectedTab.Document.Name, _selectedTab.Document.Read());
            }
        }
    }

    private double BodyFontSize
    {
        get => bodyFontSize;
        set
        {
            if (Math.Abs(bodyFontSize - value) < 0.1f || value < 8) return;
            bodyFontSize = value;
            if (BodyBlock.CheckAccess()) BodyBlock.FontSize = bodyFontSize;
            else BodyBlock.Dispatcher.Invoke(() => BodyBlock.FontSize = bodyFontSize);
        }
    }
    
    public HelpWindow()
    {
        InitializeComponent();
        UpdateContentList();
    }

    private void UpdateContentList()
    {
        foreach (var topDirectory in DocumentationManager.Tree.Root.Children)
        {
            DocumentListItem item = new DocumentListItem(this, topDirectory);
            if (ContentList.CheckAccess())
            {
                ContentList.Children.Add(item);
            }
            else
            {
                ContentList.Dispatcher.Invoke(() =>
                {
                    ContentList.Children.Add(item);
                });
            }
        }
    }

    private void SetDisplayContent(string title, string content)
    {
        if (CheckAccess())
        {
            TitleBlock.Text = title;
            BodyBlock.Text = content;
        }
        else
        {
            Dispatcher.Invoke(() =>
            {
                TitleBlock.Text = title;
                BodyBlock.Text = content;
            });
        }
    }
    
    public void CloseTabRequest(DocumentTab tab)
    {
        tab.Document.Flush();
        RemoveDocumentTab(tab);
    }
    public void SelectTabRequest(DocumentTab tab)
    {
        SelectedTab = tab;
    }
    public void OpenTabRequest(DiskDocument document)
    {
        var tab = GetDocumentTab(document) ?? CreateDocumentTab(document);
        SelectedTab = tab;
    }

    private DocumentTab CreateDocumentTab(DiskDocument document)
    {
        DocumentTab tab = new DocumentTab(document, this);
        OpenTabs.Add(tab);
        if (DocumentTabsList.CheckAccess()) DocumentTabsList.Children.Add(tab);
        else DocumentTabsList.Dispatcher.Invoke(() => DocumentTabsList.Children.Add(tab));
        return tab;
    }
    private void RemoveDocumentTab(DocumentTab tab)
    {
        if (SelectedTab == tab) SelectTabNextTo(tab, -1, false);
        OpenTabs.Remove(tab);
        if (DocumentTabsList.CheckAccess()) DocumentTabsList.Children.Remove(tab);
        else DocumentTabsList.Dispatcher.Invoke(() => DocumentTabsList.Children.Remove(tab));
    }
    private DocumentTab? GetDocumentTab(DiskDocument document)
    {
        return OpenTabs.Find(tab => tab.MatchDocument(document));
    }
    
    private void SelectTabNextTo(DocumentTab tab, int direction, bool includeSelf = true)
    {
        if (DocumentTabsList.CheckAccess()) SelectTabNextToAction(tab, direction, includeSelf);
        else DocumentTabsList.Dispatcher.Invoke(() => SelectTabNextToAction(tab, direction, includeSelf));
    }
    private void SelectTabNextToAction(DocumentTab tab, int direction, bool includeSelf = true)
    {
        var count = DocumentTabsList.Children.Count;
        switch (count)
        {
            case 0:
                SelectedTab = null;
                return;
            case 1:
                SelectedTab = includeSelf ? DocumentTabsList.Children[0] as DocumentTab : null;
                return;
        }

        direction = int.Sign(direction);
        int index = (DocumentTabsList.Children.IndexOf(tab) + direction) % count;
        SelectedTab = DocumentTabsList.Children[index] as DocumentTab;
    }
    
    private void BodyScrollViewer_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (!Keyboard.IsKeyDown(Key.LeftCtrl)) return;
        
        BodyFontSize += 4 * int.Sign(e.Delta);
        e.Handled = true;
    }
    private void DragBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}