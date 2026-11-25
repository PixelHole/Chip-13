using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Config.Edit_Window;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Config;

public class InstructionSetEditControl
{
    private InstructionSetEdit View { get; set; }
    private InstructionTemplateEditWindow? EditWindow { get; set; }


    public InstructionSetEditControl(InstructionSetEdit view)
    {
        View = view;
    }

    public void ShowEditWindow(InstructionTemplate template, bool create = false)
    {
        if (EditWindow != null)
        {
            EditWindow.Template = template;
            WPFUtility.BringWindowToTop(EditWindow);
            return;
        }

        EditWindow = new InstructionTemplateEditWindow(template);
        EditWindow.Closed += (_, _) => EditWindow = null;
        if (!create) EditWindow.OnConfirm += EditWindowOnConfirm;
        else EditWindow.OnConfirm += CreateWindowOnConfirm;
        EditWindow.Show();
    }

    public void DeleteTemplate(InstructionTemplate template)
    {
        ConfirmationPrompt prompt = new ConfirmationPrompt("Are you sure?",
            "Are you sure you want to delete this template?", ["Yes", "No"],
            s =>
            {
                if (s != "Yes") return;
                View.AssignedSet?.RemoveTemplate(template);
                View.UpdateListItems();
            });
        prompt.Show();
    }
    
    private void EditWindowOnConfirm(InstructionTemplateEditWindow sender, InstructionTemplate template)
    {
        View.UpdateListItems();
    }
    private void CreateWindowOnConfirm(InstructionTemplateEditWindow sender, InstructionTemplate template)
    {
        View.AssignedSet?.AddTemplate(template);
        View.UpdateListItems();
    }
}