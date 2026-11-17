using System.Windows;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Config.Edit_Window;

public partial class InstructionTemplateEditWindow : Window
{
    public static readonly DependencyProperty FrameTitleProperty = DependencyProperty.Register(
        nameof(FrameTitle),
        typeof(string),
        typeof(InstructionTemplateEditWindow),
        new PropertyMetadata("Edit Instruction")
        );
    
        
        
    private InstructionTemplate _template;

    public InstructionTemplate Template
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
        
    }

    public bool NameCondition(string s)
    {
        return true;
    }
    public bool HeaderCondition(string s)
    {
        return true;
    }
    private void NameBox_OnTextConfirmed(string text)
    {
        throw new NotImplementedException();
    }
    private void HeaderBox_OnTextConfirmed(string text)
    {
        throw new NotImplementedException();
    }
}