using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite.Computer.Instructions;

namespace Chippie_Lite.Internal
{
    public static class AssemblyUtility
    {
        public static InstructionArgument ParseArgument(string arg)
        {
            Register? register = null;
            int val = 0;
        
            string body = ExtractMainBody(arg);

            if (body.StartsWith('$'))
            {
                register = ExtractRegister(body);
            }
            else
            {
                val = NumberUtility.ParseNumber(body);
            }

            string offset = ExtractOffset(arg);

            if (!string.IsNullOrEmpty(offset))
            {
                val += NumberUtility.ParseNumber(offset);
            }

            return new InstructionArgument(val, register);
        }

        private static Register? ExtractRegister(string arg)
        {
            string registerId = arg.Substring(1);
        
            var register = NumberUtility.TryParseNumber(registerId, out int index)
                ? RegisterBank.GetRegister(index)
                : RegisterBank.GetRegisterById(registerId);
        
            if (register == null) throw new ArgumentException();

            return register;
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