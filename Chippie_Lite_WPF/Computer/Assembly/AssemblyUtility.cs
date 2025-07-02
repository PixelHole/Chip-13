using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Instructions.Arguments.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;

namespace Chippie_Lite_WPF.Computer.Internal
{
    public static class AssemblyUtility
    {
        public static AssemblyLabel ParseLabel(string line, int InstructionIndex)
        {
            line = line[1..];

            if (string.IsNullOrWhiteSpace(line)) throw new InvalidInstructionException("Label must have a name", 0);

            if (!line.Contains('=')) return new AssemblyLabel(line, InstructionIndex + 1);
            
            string[] parts = line.Split('=', StringSplitOptions.TrimEntries);

            if (string.IsNullOrEmpty(parts[0])) throw new InvalidInstructionException("Label must have a name", 0);
                
            int value = 0;
                
            try
            {
                value = NumberUtility.ParseNumber(parts[1]);
            }
            catch (Exception)
            {
                throw new InvalidInstructionException("Invalid label Value", 0);
            }

            return new AssemblyLabel(parts[0], value);

        }

        public static InstructionArgument ParseArgument(string arg, InstructionArgumentType argType)
        {
            if (string.IsNullOrEmpty(arg)) throw new InvalidArgumentException(0, arg, argType);
            
            switch (argType)
            {
                case InstructionArgumentType.RegisterAndNumber :
                    return ParseRegisterNumberArgument(arg);
                
                case InstructionArgumentType.Number :
                    return ParseNumberArgument(arg);
                
                case InstructionArgumentType.Register :
                    return ParseRegisterArgument(arg);
                
                default :
                    throw new InvalidArgumentTypeException(0, argType);
            }
        }
        private static RegisterNumberArgument ParseRegisterNumberArgument(string arg)
        {
            string body = ExtractMainBody(arg);

            if (!body.StartsWith('$')) return new RegisterNumberArgument(null, ParseNumber(body));
            
            var register = ParseRegister(body);

            string offset = ExtractOffset(arg);

            int val = string.IsNullOrEmpty(offset) ? 0 : ParseNumber(offset);

            return new RegisterNumberArgument(register, val);
        }
        private static RegisterArgument ParseRegisterArgument(string arg)
        {
            return new RegisterArgument(ParseRegister(arg));
        }
        private static NumberArgument ParseNumberArgument(string arg)
        {
            return new NumberArgument(ParseNumber(arg));
        }

        private static Register? ParseRegister(string arg)
        {
            string registerId = arg;

            switch (registerId[0])
            {
                case '$' :
                    registerId = registerId[1..];
                    break;
                
                case '@' :
                    var label = AssemblyLabelDatabase.GetLabel(registerId[1..]);
                    if (label == null) throw new InvalidInstructionException("Invalid label name", 0);
                    registerId = label.Value.ToString();
                    break;
            }

            var register = NumberUtility.TryParseNumber(registerId, out int index)
                ? RegisterBank.GetRegister(index)
                : RegisterBank.GetRegisterById(registerId);
        
            if (register == null) throw new RegisterNotFoundException(0, registerId);

            return register;
        }
        private static int ParseNumber(string raw)
        {
            if (!raw.StartsWith('@')) return NumberUtility.ParseNumber(raw);
            
            var label = AssemblyLabelDatabase.GetLabel(raw[1..]);
            if (label == null) throw new InvalidInstructionException("Invalid label name", 0);
            return label.Value;

        }
        
        private static string ExtractOffset(string arg)
        {
            if (!arg.Contains('[')) return "";
            return arg.Substring(arg.IndexOf('[') + 1, arg.IndexOf(']') - arg.IndexOf('[') - 1);
        }
        private static string ExtractMainBody(string arg)
        {
            if (!arg.Contains('[')) return arg;
            return arg.Substring(0, arg.IndexOf('['));
        }
    }
}