using System.Text;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;

namespace Chippie_Lite_WPF.Computer.Instructions;

public struct Instruction
{
    public string Header { get; private set; }
    public List<InstructionArgument> Arguments { get; private set; } = new List<InstructionArgument>();
    public List<InstructionAction> Actions { get; private set; } = new List<InstructionAction>();
    public int ScriptLine { get; set; }


    public Instruction(string header, IEnumerable<InstructionArgument> arguments, IEnumerable<InstructionAction> actions)
    {
        Header = header;
        AddArguments(arguments);
        AddActions(actions);
    }

    private void AddArguments(IEnumerable<InstructionArgument> arguments)
    {
        foreach (var argument in arguments)
        {
            Arguments.Add(argument);
        }
    }
    private void AddActions(IEnumerable<InstructionAction> actions)
    {
        foreach (var action in actions)
        {
            Actions.Add(action);
        }
    }

    public override string ToString()
    {
        StringBuilder insText = new StringBuilder(Header);

        if (Arguments.Count > 0)
        {
            insText.Append($" : [{Arguments[0]}]");

            for (int i = 1; i < Arguments.Count; i++)
            {
                insText.Append($" , [{Arguments[i].ToString()}]");
            }
        }

        return insText.ToString();
    }
}