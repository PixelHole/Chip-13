using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Cat;

namespace Chippie_Lite_WPF.Computer.Components
{
    public static class InstructionInterpreter
    {
        public static void Interpret(Instruction instruction)
        {
            for (int i = 0; i < instruction.Actions.Count; i++)
            {
                var action = instruction.Actions[i];
                try
                {
                    if (InstructionActionConditions.EvaluateCondition(action)) InstructionActions.ExecuteAction(action);
                }
                catch (InstructionInterpretationException e)
                {
                    e.Action = action;
                    e.Instruction = instruction;
                    e.ActionIndex = i;
                    throw;
                }
            }
        }
    }
}