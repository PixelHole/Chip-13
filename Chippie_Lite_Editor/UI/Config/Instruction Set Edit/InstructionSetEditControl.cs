using System.Windows;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Config.Edit_Window;
using Chippie_Lite_WPF.UI.Elements;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Config;

public class InstructionSetEditControl
{
    private InstructionSetEdit View { get; set; }
    private InstructionTemplateEditWindow? EditWindow { get; set; }


    public InstructionSetEditControl(InstructionSetEdit view)
    {
        View = view;
    }

    public void ShowEditWindow(InstructionTemplate template)
    {
        if (EditWindow != null)
        {
            EditWindow.Template = template;
            WPFUtility.BringWindowToTop(EditWindow);
            return;
        }

        EditWindow = new InstructionTemplateEditWindow(template);
        EditWindow.Closed += (_, _) => EditWindow = null;
        EditWindow.Show();
    }
    
    
    public void DeleteTemplate(InstructionTemplate template)
    {
        ConfirmationPrompt prompt = new ConfirmationPrompt("Are you sure?",
            "Are you sure you want to delete this template?", ["Yes", "No"]);
        prompt.OnButtonSelected += choice => PromptOnOnButtonSelected(choice, template);
        prompt.Show();
    }
    private void PromptOnOnButtonSelected(string choice, InstructionTemplate template)
    {
        if (choice != "Yes") return;
        View.AssignedSet?.RemoveTemplate(template);
        View.UpdateListItems();
    }
}