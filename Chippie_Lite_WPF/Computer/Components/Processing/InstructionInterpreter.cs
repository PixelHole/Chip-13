using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components
{
    public static class InstructionInterpreter
    {
        public static void Interpret(Instruction instruction)
        {
            foreach (var action in instruction.Actions)
            {
                InstructionActions.ExecuteAction(action, instruction.Arguments);
            }
        }
    }
}