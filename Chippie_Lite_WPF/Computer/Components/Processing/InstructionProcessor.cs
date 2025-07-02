using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components
{
    public static class InstructionProcessor
    { 
        public static void ExecuteNextInstruction(Register pointer)
        {
            Instruction instruction = InstructionBank.GetInstruction(pointer.Content);
            try
            {
                InstructionInterpreter.Interpret(instruction);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            pointer.AddToContent(1);
        }
    }
}