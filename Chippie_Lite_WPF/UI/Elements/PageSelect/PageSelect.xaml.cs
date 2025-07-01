using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Chippie_Lite_WPF.UI.Elements;


public partial class PageSelect : UserControl
{
    public delegate void PageChangedAction(int index);
    public event PageChangedAction OnPageChanged;

    private List<PageTab> Tabs = new List<PageTab>();
    private PageTab? SelectedTab { get; set; }
    public int SelectedTabIndex => Tabs.IndexOf(SelectedTab);

    public PageSelect()
    {
        InitializeComponent();
        SetupTabs();
        SelectedTab = Tabs[0];
        SelectedTab.SetSelected(true);
    }

    private void SetupTabs()
    {
        Tabs.Clear();
        
        foreach (UIElement child in Body.Children)
        {
            if (child is not PageTab tab) continue;
            Tabs.Add(tab);
            SelectedTab ??= tab;
            tab.OnSelected += OnTabSelected;
        }
    }

    private void OnTabSelected(PageTab tab)
    {
        SelectedTab.SetSelected(false);
        SelectedTab = tab;
        OnPageChanged?.Invoke(SelectedTabIndex);
    }
}