using Chippie_Lite.Computer.Components.InstructionBanks;
using Chippie_Lite.Computer.Components.Registers;
using Chippie_Lite.Computer.Instructions;
using Chippie_Lite.Internal;

namespace Chippie_Lite.Computer.Components.Processing
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