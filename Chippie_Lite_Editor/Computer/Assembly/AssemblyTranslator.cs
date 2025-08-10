using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Assembly
{
    public static class AssemblyTranslator
    { 
        public static List<Instruction> ParseScript(string raw)
        {
            AssemblyLabelDatabase.ClearLabels();
            string[] lines = raw.Replace("\r", "").Split('\n');

            ExtractAllLabels(lines);

            return ExtractAllInstructions(lines);
        }
        
        private static void ExtractAllLabels(string[] lines)
        {
            int actualLine = -1;
            int instructionIndex = 0;
            
            foreach (var line in lines)
            {
                actualLine++;
                if (string.IsNullOrEmpty(line)) continue;
                if (!line.StartsWith('#') && !line.StartsWith('@'))
                {
                    instructionIndex++;
                    continue;
                }

                AssemblyLabel label;
                
                try
                {
                    label = AssemblyUtility.ParseLabel(line, instructionIndex);
                }
                catch (InvalidInstructionException e)
                {
                    e.SetLine(actualLine);
                    throw;
                }
                
                if (AssemblyLabelDatabase.AddLabel(label)) return; 
                
                throw new InvalidInstructionException(actualLine);
            }
        }

        private static List<Instruction> ExtractAllInstructions(string[] lines)
        {
            List<Instruction> instructions = new List<Instruction>(); 
            
            int actualLine = -1;
            int instructionIndex = 0;
            
            foreach (var line in lines)
            {
                actualLine++;
                if (line.StartsWith('#') || string.IsNullOrEmpty(line) || line.StartsWith('@')) continue;
                try
                {
                    Instruction instruction = ParseInstruction(line);
                    instruction.ScriptLine = actualLine;
                    instructions.Add(instruction);
                }
                catch (InvalidInstructionException e)
                {
                    e.SetLine(actualLine);
                    throw;
                }
                instructionIndex++;
            }
            
            return instructions;
        }
        private static Instruction ParseInstruction(string raw)
        {
            raw = raw.Trim();
            bool hasBr = false;
            if (raw.StartsWith('>'))
            {
                raw = raw[1..].TrimStart();
                hasBr = true;
            }
            
            string[] parts = raw.Split(' ', '\t', StringSplitOptions.RemoveEmptyEntries);
            string header = parts[0].Trim();

            var template = InstructionSet.FindTemplate(header);

            if (template == null) throw new InvalidHeaderException(0, header);
            
            if (parts.Length == 1 && template.ArgumentTypes.Length != 0)
                throw new NotEnoughArgumentsException(0, template.ArgumentTypes.Length, 0);
            
            List<InstructionArgument> arguments = [];

            string[] args = ExtractArguments(raw);

            for (int i = 0; i < template.ArgumentTypes.Length; i++)
            {
                string arg = "";
                    
                try
                {
                    arg = args[i];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new NotEnoughArgumentsException(0, template.ArgumentTypes.Length, args.Length);
                }

                var type = template.ArgumentTypes[i];
                
                arguments.Add(AssemblyUtility.ParseArgument(arg, type));
            }

            return new Instruction(header, arguments, template.Actions, hasBr);
        }
        

        private static string[] ExtractArguments(string instruction)
        {
            if (!instruction.Contains(' ')) return [];
             return instruction.Substring(instruction.IndexOf(' '), instruction.Length - instruction.IndexOf(' '))
                 .Trim().Split(',', StringSplitOptions.TrimEntries);
        }
    }
}