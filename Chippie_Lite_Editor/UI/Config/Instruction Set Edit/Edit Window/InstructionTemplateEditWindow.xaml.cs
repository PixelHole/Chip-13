using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Config.Edit_Window;

public partial class InstructionTemplateEditWindow
{
    public static readonly DependencyProperty FrameTitleProperty = DependencyProperty.Register(
        nameof(FrameTitle),
        typeof(string),
        typeof(InstructionTemplateEditWindow),
        new PropertyMetadata("Edit Instruction")
        );
        
    private InstructionTemplate _template = null!;

    public new InstructionTemplate Template
    {
        get => _template;
        set
        {
            _template = value;
            LoadTemplateData();
        }
    }

    public string FrameTitle
    {
        get => (string)GetValue(FrameTitleProperty);
        set => SetValue(FrameTitleProperty, value);
    }

    public delegate void ConfirmAction(InstructionTemplateEditWindow sender, InstructionTemplate template);
    public event ConfirmAction? OnConfirm;
    
    public InstructionTemplateEditWindow()
    {
        InitializeComponent();
        Owner = MainWindow.MainWindowIns;
    }
    public InstructionTemplateEditWindow(InstructionTemplate template) : this()
    {
        Template = template;
    }

    private void LoadTemplateData()
    {
        NameBox.Text = Template.Name;
        HeaderBox.Text = Template.Header;
        UpdateTemplateArguments();
        UpdateTemplateActions();
    }
    
    private void UpdateTemplateArguments()
    {
        ArgumentTypePanel.Children.Clear();
        while (ArgumentTypePanel.Children.Count != Template.ArgumentTypes.Length)
        {
            if (ArgumentTypePanel.Children.Count < Template.ArgumentTypes.Length)
            {
                ArgumentListItem item = new ArgumentListItem();
                item.OnTypeSelected += ArgumentTypeSelected;
                item.OnDeleteRequested += ArgumentDeleteRequested;
                item.OnDuplicateRequested += ArgumentDuplicateRequested;
                ArgumentTypePanel.Children.Add(item);
            }
            else 
            {
                var item = (ArgumentTypePanel.Children[^1] as ArgumentListItem)!;
                item.OnTypeSelected -= ArgumentTypeSelected;
                item.OnDeleteRequested -= ArgumentDeleteRequested;
                item.OnDuplicateRequested -= ArgumentDuplicateRequested;
                ArgumentTypePanel.Children.Remove(item);
            }
        }

        for (int i = 0; i < ArgumentTypePanel.Children.Count; i++)
        {
            var item = (ArgumentTypePanel.Children[i] as ArgumentListItem)!;
            item.SelectedIndex = (int)Template.ArgumentTypes[i];
        }
    }
    private void UpdateTemplateActions()
    {
        ActionsPanel.Children.Clear();
        while (ActionsPanel.Children.Count != Template.Actions.Length)
        {
            if (ActionsPanel.Children.Count < Template.Actions.Length)
            {
                ActionListItem item = new ActionListItem(this) {
                    Margin = new Thickness(0, 4, 0, 0)
                };
                item.OnDeleteRequest += ActionDeleteRequest;
                item.OnDuplicateRequest += ActionDuplicateRequest;
                ActionsPanel.Children.Add(item);
            }
            else 
            {
                var item = (ActionsPanel.Children[^1] as ActionListItem)!;
                item.OnDeleteRequest -= ActionDeleteRequest;
                item.OnDuplicateRequest -= ActionDuplicateRequest;
                ActionsPanel.Children.Remove(item);
            }
        }

        for (int i = 0; i < ActionsPanel.Children.Count; i++)
        {
            var item = (ActionsPanel.Children[i] as ActionListItem)!;
            item.Action = Template.Actions[i];
        }
    }
    
    private void ActionDuplicateRequest(ActionListItem sender, InstructionAction action)
    {
        Template.Actions = Template.Actions.Append(action).ToArray();
        UpdateTemplateActions();
    }
    private void ActionDeleteRequest(ActionListItem sender, InstructionAction action)
    {
        Template.Actions = Template.Actions.Where(a => !a.Equals(action)).ToArray();
        UpdateTemplateActions();
    }

    private void ArgumentDuplicateRequested(ArgumentListItem sender, InstructionArgumentType type)
    {
        int index = ArgumentTypePanel.Children.IndexOf(sender);
        if (index == -1) return;
        var newTypes = new InstructionArgumentType[Template.ArgumentTypes.Length + 1];
        for (int i = 0; i < newTypes.Length; i++)
        {
            if (i < index) newTypes[i] = Template.ArgumentTypes[i];
            else if (i == index) newTypes[i] = type;
            else newTypes[i] = Template.ArgumentTypes[i - 1];
        }
        Template.ArgumentTypes = newTypes;
        
        ArgumentListItem item = new ArgumentListItem();
        item.OnTypeSelected += ArgumentTypeSelected;
        item.OnDeleteRequested += ArgumentDeleteRequested;
        item.OnDuplicateRequested += ArgumentDuplicateRequested;
        ArgumentTypePanel.Children.Insert(index + 1, item);
        item.SelectedIndex = (int)type;
    }
    private void ArgumentDeleteRequested(ArgumentListItem sender, InstructionArgumentType type)
    {
        int index = ArgumentTypePanel.Children.IndexOf(sender);
        if (index == -1) return;
        var newTypes = new InstructionArgumentType[Template.ArgumentTypes.Length - 1];
        for (int i = 0; i < newTypes.Length; i++)
        {
            if (i < index) newTypes[i] = Template.ArgumentTypes[i];
            else newTypes[i] = Template.ArgumentTypes[i + 1];
        }
        Template.ArgumentTypes = newTypes;
        var item = (ArgumentTypePanel.Children[index] as ArgumentListItem)!;
        item.OnTypeSelected -= ArgumentTypeSelected;
        item.OnDeleteRequested -= ArgumentDeleteRequested;
        item.OnDuplicateRequested -= ArgumentDuplicateRequested;
        ArgumentTypePanel.Children.Remove(item);
    }
    private void ArgumentTypeSelected(ArgumentListItem sender, InstructionArgumentType type)
    {
        int index = ArgumentTypePanel.Children.IndexOf(sender);
        if (index < 0) return;
        Template.ArgumentTypes[index] = type;
    }

    private bool ConfirmEdit()
    {
        if (string.IsNullOrEmpty(NameBox.Text))
        {
            ErrorBox.Pop("Name cant be empty!", "You must enter a name for the instruction!");
            return false;
        }

        if (string.IsNullOrEmpty(HeaderBox.Text))
        {
            ErrorBox.Pop("Header cant be empty!", "You must enter a header for the instruction!");
            return false;
        }
        
        OnConfirm?.Invoke(this, Template);
        return true;
    }
    
    public bool NameCondition(string s)
    {
        if (string.IsNullOrWhiteSpace(s) || !s.All(c => char.IsLetterOrDigit(c) || c == ' ')) return false;
        var match = InstructionSetManager.ActiveInstructionSet.FindTemplate(template => template.Name == s);
        return match == null || match.Equals(Template);
    }
    public bool HeaderCondition(string s)
    {
        if (string.IsNullOrWhiteSpace(s) || !s.All(char.IsLetter)) return false;
        var match = InstructionSetManager.ActiveInstructionSet.FindTemplate(s);
        return match == null || match.Equals(Template);
    }
    
    private void NameBox_OnTextConfirmed(string text)
    {
        Template.Name = text;
    }
    private void HeaderBox_OnTextConfirmed(string text)
    {
        Template.Header = text;
    }
    private void OkBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        if (ConfirmEdit()) Close();
    }
    private void CancelBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Close();
    }
    private void ActionAddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Template.Actions = Template.Actions.Append(new InstructionAction("Add", ["", "", ""])).ToArray();
        UpdateTemplateActions();
    }
    private void ArgumentAddBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        Template.ArgumentTypes = Template.ArgumentTypes.Append(InstructionArgumentType.RegisterAndValue).ToArray();
        UpdateTemplateArguments();
    }
}