using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config.Edit_Window;

public partial class ActionListItem : UserControl
{
    public InstructionAction _action;
    public InstructionAction Action
    {
        get => _action;
        set
        {
            _action = value;
            UpdateData();
        }
    }
    
    private InstructionTemplateEditWindow View { get; set; }
    
    
    public delegate void DuplicateAction(ActionListItem sender, InstructionAction action);
    public delegate void DeleteAction(ActionListItem sender, InstructionAction action);
    public event DuplicateAction? OnDuplicateRequest;
    public event DeleteAction? OnDeleteRequest;
    
    private bool UpdatingData { get; set; } = false;
    
    public ActionListItem(InstructionTemplateEditWindow view)
    {
        InitializeComponent();
        LoadInternalActions();
        View = view;
    }
    private void LoadInternalActions()
    {
        ActionTypeBox.Items.Clear();
        foreach (var actionType in InstructionActions.Actions)
        {
            ActionTypeBox.Items.Add(actionType.Header);
        }
    }
    
    private void UpdateData()
    {
        UpdateActionType();
        UpdateTextboxGrid();
        UpdateConditionTextbox();
    }
    private void UpdateActionType()
    {
        UpdatingData = true;
        int index = InstructionActions.IndexOf(Action.Header);
        if (index >= 0) ActionTypeBox.SelectedIndex = index;
        UpdatingData = false;
    }
    private void UpdateTextboxGrid()
    {
        var internalAction = InstructionActions.FindAction(Action.Header);
        if (internalAction == null) 
            throw new InternalActionNotFoundException($"Internal action with header '{Action.Header}' not found.", Action.Header);
        int count = internalAction.ExpectedArgumentCount;

        while (TextboxGrid.Children.Count != count)
        {
            if (TextboxGrid.Children.Count < count)
            {
                TextboxGrid.ColumnDefinitions.Add(new ColumnDefinition());
                var textbox = new ActionListItemTextbox();
                textbox.OnTextConfirmed += TextboxOnOnTextConfirmed;
                TextboxGrid.Children.Add(textbox);
                Grid.SetColumn(textbox, TextboxGrid.Children.Count - 1);
            }
            else
            {
                var item = (TextboxGrid.Children[^1] as ActionListItemTextbox)!;
                item.OnTextConfirmed -= TextboxOnOnTextConfirmed;
                TextboxGrid.Children.Remove(item);
                TextboxGrid.ColumnDefinitions.RemoveAt(TextboxGrid.ColumnDefinitions.Count - 1);
            }
        }
        
        for (int i = 0; i < TextboxGrid.Children.Count; i++)
        {
            var item = (TextboxGrid.Children[i] as ActionListItemTextbox)!;
            item.Margin = i == TextboxGrid.Children.Count - 1 ? new Thickness(0, 0, 0, 0) : new Thickness(0, 0, 4, 0);
            item.Text = Action.Arguments[i];
            item.Placeholder = internalAction.ArgumentNames[i];
        }
    }
    private void UpdateConditionTextbox()
    {
        ConditionBox.Text = Action.Condition;
    }
    
    private void TextboxOnOnTextConfirmed(ActionListItemTextbox sender, string text)
    {
        int index = TextboxGrid.Children.IndexOf(sender);
        if (index == -1) return;
        Action.Arguments[index] = text;
    }
    private void ConditionBox_OnTextConfirmed(string text)
    {
        Action.Condition = text;
    }
    private void ActionTypeBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UpdatingData) return;
        string header = e.AddedItems[0] as string ?? throw new InvalidOperationException("Selected item is not a string.");
        var internalAction = InstructionActions.FindAction(header);
        Action.Header = (string)header;
        Action.ClearArguments();
        UpdateTextboxGrid();
    }
    
    public bool ActionCondition(string s) => true;
    
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        OnDuplicateRequest?.Invoke(this, Action);
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        OnDeleteRequest?.Invoke(this, Action);
    }
    
}