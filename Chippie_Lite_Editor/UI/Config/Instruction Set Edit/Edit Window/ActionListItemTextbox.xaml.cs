using System.Windows.Controls;

namespace Chippie_Lite_WPF.UI.Config.Edit_Window;

public partial class ActionListItemTextbox : UserControl
{
    public delegate void TextConfirmAction(ActionListItemTextbox sender, string text);
    public event TextConfirmAction? OnTextConfirmed;
    
    public string Text 
    {
        get => Textbox.Text;
        set => Textbox.Text = value;
    }
    public string Placeholder
    {
        get => Textbox.Placeholder;
        set => Textbox.Placeholder = value;
    }
    
    
    public ActionListItemTextbox()
    {
        InitializeComponent();
    }

    public bool Condition(string s) => true;
    private void Textbox_OnTextConfirmed(string text)
    {
        OnTextConfirmed?.Invoke(this, text);
    }
}