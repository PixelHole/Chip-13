using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components
{
    public static class InstructionProcessor
    { 
        public static void ExecuteNextInstruction(int index)
        {
            Instruction instruction = InstructionBank.GetInstruction(index);
            InstructionInterpreter.Interpret(instruction);
        }
    }
}