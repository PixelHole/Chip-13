using Chippie_Lite.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components
{
    public static class InstructionInterpreter
    {
        public static void Interpret(Instruction instruction)
        {
            switch (instruction.Header.ToLower())
            {
                case "add":
                    Add(instruction.Arguments);
                    break;
            }
        }

        private static void Add(IList<InstructionArgument> args)
        {
            Register dump = args[0].GetRegister();

            int a = args[1].GetValue();
            int b = args[2].GetValue();
        
            dump.SetContent(a);
            dump.AddToContent(b);
        }
    }
}