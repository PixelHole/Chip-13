using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Chippie_Lite_WPF.UI.Elements;


public partial class PageSelect : UserControl
{
    public delegate void PageChangedAction(int index);
    public event PageChangedAction? OnPageChanged;

    private readonly List<PageTab> Tabs = new List<PageTab>();
    private PageTab? selectedTab;

    private PageTab? SelectedTab
    {
        get
        {
            return selectedTab;
        }
        set
        {
            if (selectedTab != null) selectedTab.Selected = false;
            selectedTab = value;
            if (selectedTab != null) selectedTab.Selected = true;
        }
    }

    public int SelectedTabIndex => SelectedTab == null ? -1 : Tabs.IndexOf(SelectedTab);

    public PageSelect()
    {
        InitializeComponent();
        SetupTabs();
        SelectedTab = Tabs[0];
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

    public void ChangeSelectedTab(int index)
    {
        SelectedTab = Tabs[index];
        OnPageChanged?.Invoke(index);
    }
    
    private void OnTabSelected(PageTab tab)
    {
        SelectedTab = tab;
        OnPageChanged?.Invoke(SelectedTabIndex);
    }
}