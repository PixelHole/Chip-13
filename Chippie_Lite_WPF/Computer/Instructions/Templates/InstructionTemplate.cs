namespace Chippie_Lite_WPF.Computer.Instructions.Templates;

public class InstructionTemplate
{
    public string Name { get; private set; }
    public string Header { get; private set; }
    public InstructionArgumentType[] ArgumentTypes { get; private set; }
    public InstructionAction[] Actions { get; private set; }


    public InstructionTemplate(string name, string header, InstructionArgumentType[] argumentTypes, InstructionAction[] actions)
    {
        Name = name;
        Header = header;
        ArgumentTypes = argumentTypes;
        Actions = actions;
    }
}