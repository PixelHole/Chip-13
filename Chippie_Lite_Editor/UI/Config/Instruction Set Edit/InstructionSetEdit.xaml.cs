using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config;

public partial class InstructionSetEdit
{
    public delegate void EditCancelAction(InstructionSetEdit sender, InstructionSet? assignedSet);
    public delegate void EditConfirmAction(InstructionSetEdit sender, InstructionSet? assignedSet);
    public event EditCancelAction? OnCancel;
    public event EditConfirmAction OnConfirm;
    
    private InstructionSetEditControl Control;
    
    private InstructionSet? assignedSet;
    private string searchTerm = string.Empty;

    public InstructionSet? AssignedSet
    {
        get => assignedSet;
        set
        {
            assignedSet = value;
            OnSetChanged();
        }
    }

    public string SearchTerm
    {
        get => searchTerm;
        set
        {
            if (searchTerm == value) return;
            searchTerm = value;
            if (!string.IsNullOrWhiteSpace(searchTerm)) UpdateListItems();
        }
    }
    
    
    public InstructionSetEdit()
    {
        InitializeComponent();
        ConnectEvents();
        Control = new InstructionSetEditControl(this);
        AssignedSet = InstructionSetManager.ActiveInstructionSet;
    }
    private void ConnectEvents()
    {
        InstructionSetManager.OnActiveSetChanged += (set, index) => AssignedSet = set;
    }
    
    public bool SearchCondition(string s) => true;
    
    private void OnSetChanged()
    {
        UpdateTitle();
        NameBox.Text = AssignedSet?.Name ?? "";
        UpdateListItems();
    }
    private void UpdateTitle()
    {
        Frame.Title = AssignedSet?.Name ?? "No set loaded";
    }
    public void UpdateListItems()
    {
        if (AssignedSet == null || AssignedSet.Count == 0)
        {
            TemplatesPanel.Children.Clear();
            return;
        }

        var set = ApplyFilters(AssignedSet.GetTemplates());

        while (TemplatesPanel.Children.Count != set.Length)
        {
            if (TemplatesPanel.Children.Count > set.Length) TemplatesPanel.Children.RemoveAt(TemplatesPanel.Children.Count - 1);
            else TemplatesPanel.Children.Add(new TemplateListItem(this));
        }

        for (int i = 0; i < TemplatesPanel.Children.Count; i++)
        {
            var item = TemplatesPanel.Children[i] as TemplateListItem;
            item!.Template = set[i];
        }
    }
    private InstructionTemplate[] ApplyFilters(InstructionTemplate[] templates)
    {
        var result = (InstructionTemplate[])templates.Clone();
        if (!string.IsNullOrWhiteSpace(SearchTerm)) result = SortBySimilarity(result);
        return result;
    }
    
    public void DeleteRequest(TemplateListItem sender, InstructionTemplate template)
    {
        if (AssignedSet == null) return;
        Control.DeleteTemplate(template);
    }
    public void DuplicateRequest(TemplateListItem sender, InstructionTemplate template)
    {
        if (AssignedSet == null) return;
        var newTemp = new InstructionTemplate(template.Name + " Copy", template.Header + "d", template.ArgumentTypes.ToArray(),
            template.Actions.ToArray());
        AssignedSet.AddTemplate(newTemp);
        UpdateListItems();
    }
    public void EditRequest(TemplateListItem sender, InstructionTemplate template)
    {
        Control.ShowEditWindow(template);
    }
    
    public InstructionTemplate[] SortBySimilarity(InstructionTemplate[] input)
    {
        if (input.Length == 0 || string.IsNullOrWhiteSpace(SearchTerm)) return input;

        string trimmedKeyword = SearchTerm.Trim();

        return input
            .OrderBy(item => GetMatchTier(item.Name, trimmedKeyword))
            .ThenBy(item => GetMatchTier(item.Header, trimmedKeyword))
            .ThenBy(item => ComputeLevenshteinDistance(item.Name, trimmedKeyword))
            .ThenBy(item => ComputeLevenshteinDistance(item.Header, trimmedKeyword))
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Header, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
    private static int GetMatchTier(string? item, string keyword)
    {
        if (string.IsNullOrEmpty(item))
            return 4;

        if (item.Equals(keyword, StringComparison.OrdinalIgnoreCase))
            return 0;

        if (item.StartsWith(keyword, StringComparison.OrdinalIgnoreCase))
            return 1;

        if (item.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            return 2;

        return 3;
    }
    public static int ComputeLevenshteinDistance(string? source, string? target)
    {
        if (string.IsNullOrEmpty(source)) return target?.Length ?? 0;
        if (string.IsNullOrEmpty(target)) return source.Length;

        int n = source.Length;
        int m = target.Length;

        // Use two 1D rows instead of an n*m matrix to save memory
        int[] previousRow = new int[m + 1];
        int[] currentRow = new int[m + 1];

        for (int j = 0; j <= m; j++)
            previousRow[j] = j;

        for (int i = 1; i <= n; i++)
        {
            currentRow[0] = i;
            for (int j = 1; j <= m; j++)
            {
                int cost = char.ToLowerInvariant(source[i - 1]) == char.ToLowerInvariant(target[j - 1]) ? 0 : 1;

                currentRow[j] = Math.Min(
                    Math.Min(currentRow[j - 1] + 1, previousRow[j] + 1),
                    previousRow[j - 1] + cost);
            }

            (previousRow, currentRow) = (currentRow, previousRow);
        }

        return previousRow[m];
    }
    
    public bool NameCondition(string s)
    {
        if (AssignedSet == null) return true;
        if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s)) return false;
        InstructionSet? match = InstructionSetManager.FindWithName(s);
        if (match != null && match != AssignedSet) return false;
        return true;
    }
    
    private void AddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Control.ShowEditWindow(new InstructionTemplate("", "", [], []), true);
    }
    private void SearchBox_OnTextConfirmed(string text)
    {
        SearchTerm = text;
    }
    private void ClearBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        SearchBox.Text = "";
    }
    private void NameBox_OnTextConfirmed(string text)
    {
        if (AssignedSet != null) AssignedSet.Name = text;
    }
    
    private void CancelBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        OnCancel?.Invoke(this, AssignedSet);
    }
    private void ConfirmBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (string.IsNullOrEmpty(NameBox.Text)) return;
        OnConfirm?.Invoke(this, AssignedSet);
    }
}