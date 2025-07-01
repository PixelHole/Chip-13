using Chippie_Lite.Computer.Instructions;

namespace Chippie_Lite.Internal
{
    public static class AssemblyTranslator
    {
        public static List<Instruction> TranslateMultiline(string raw)
        {
            List<Instruction> instructions = new List<Instruction>(); 
            string[] lines = raw.Replace("\r", "").Split('\n');

            foreach (var line in lines)
            {
                if (line.StartsWith('#')) continue;
                instructions.Add(Translate(line));
            }

            return instructions;
        }
        public static Instruction Translate(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw) || string.IsNullOrEmpty(raw)) throw new ArgumentException("invalid instruction");
        
            string[] parts = raw.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string header = parts[0];
            List<InstructionArgument> arguments = new List<InstructionArgument>();

            if (parts.Length > 1)
            {
                string[] args = raw.Substring(raw.IndexOf(' '), raw.Length - raw.IndexOf(' '))
                    .Split(',', StringSplitOptions.TrimEntries);

                foreach (var arg in args)
                {
                    arguments.Add(AssemblyUtility.ParseArgument(arg));
                }
            }
        
            return new Instruction(header, arguments);
        }
    }
}