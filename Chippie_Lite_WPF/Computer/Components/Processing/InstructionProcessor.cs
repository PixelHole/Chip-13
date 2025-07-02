using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

namespace Chippie_Lite_WPF.Computer.Components
{
    public static class InstructionProcessor
    { 
        public static void ExecuteNextInstruction(Register pointer)
        {
            Instruction instruction = InstructionBank.GetInstruction(pointer.Content);
            InstructionInterpreter.Interpret(instruction);
            pointer.AddToContent(1);
        }
    }
}