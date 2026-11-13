using System.Text;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.Components
{
    public static class RegisterBank
    {
        private static List<Register> Registers { get; set; } = [];
        public static int Count => Registers.Count;
        public static bool Locked { get; private set; }

        public delegate void RegisterAddAction(Register register, int index);
        public delegate void RegisterRemoveAction(Register register, int index);

        public static event RegisterAddAction? OnRegisterAdded;
        public static event RegisterRemoveAction? OnRegisterRemoved;

        public static Register InstructionPointer =>
            GetRegister("Instruction Pointer") ?? throw new RegisterNotFoundException(0, "IP");

        public static Register InputBufferCount =>
            GetRegister("Input Buffer Count") ?? throw new RegisterNotFoundException(0, "IB");


        public static bool InsertRegister(Register register, int index)
        {
            if (Locked) return false;
            if (GetRegister(register.Name) != null) return false;
            Registers.Insert(index, register);
            OnRegisterAdded?.Invoke(register, index);
            return true;
        }
        
        public static bool AddRegister(Register register)
        {
            if (Locked) return false;
            if (GetRegister(register.Name) != null) return false;
            Registers.Add(register);
            OnRegisterAdded?.Invoke(register, Count - 1);
            return true;
        }
        public static int AddRegisters(IEnumerable<Register> registers)
        {
            if (Locked) return 0;
            int count = 0;

            foreach (var register in registers)
            {
                if (AddRegister(register)) count++;
            }

            return count;
        }
        public static int AddRegisters(params Register[] registers)
        {
            if (Locked) return 0;
            int count = 0;

            foreach (var register in registers)
            {
                if (AddRegister(register)) count++;
            }

            return count;
        }
        public static bool RemoveRegister(Register register)
        {
            if (Locked) return false;
            int index = IndexOf(register);
            if (index == -1) return false;
            Registers.Remove(register);
            OnRegisterRemoved?.Invoke(register, index); 
            return true;
        }
        public static int RemoveRegisters(IEnumerable<Register> registers)
        {
            if (Locked) return 0;
        
            int count = 0;

            foreach (var register in registers)
            {
                if (RemoveRegister(register)) count++;
            }

            return count;
        }

        public static void Clear()
        {
            Registers.Clear();
        }
        public static void ResetRegisters()
        {
            foreach (var register in Registers)
            {
                register.ResetContent();
            }
        }

        public static Register[] GetAllRegisters()
        {
            Register[] registers = new Register[Registers.Count];

            for (int i = 0; i < registers.Length; i++)
            {
                registers[i] = Registers[i];
            }
            
            return registers;
        }
        
        public static Register? GetRegister(int index)
        {
            return Registers.Count == 0 ? null : Registers[IndexUtility.WrapIndex(index, Registers.Count)];
        }
        public static Register? GetRegister(string name)
        {
            return FindRegister(reg => string.Equals(reg.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }
        public static Register? GetRegisterById(string id)
        {
            return FindRegister(reg => string.Equals(reg.Id, id, StringComparison.CurrentCultureIgnoreCase));
        }

        public static List<Register> GetRegisters(IEnumerable<int> indices)
        {
            var registers = new List<Register>();
            if (Registers.Count == 0) return registers;

            registers.AddRange(indices.Select(i => GetRegister(i)!));

            return registers;
        }
        public static List<Register> GetRegisters(IEnumerable<string> names)
        {
            List<Register> registers = new List<Register>();
            if (Registers.Count == 0) return registers;

            foreach (var name in names)
            {
                var match = GetRegister(name);
                if (match != null) registers.Add(match);
            }

            return registers;
        }
        
        public static Register? FindRegister(Predicate<Register> predicate)
        {
            return Registers.Find(predicate);
        }

        public static int IndexOf(Register register)
        {
            if (Registers.Count == 0) return -1;
            return Registers.IndexOf(register);
        }
        public static int IndexOf(string name)
        {
            if (Registers.Count == 0) return -1;
            Register? match = GetRegister(name);
            if (match == null) return -1;
            return IndexOf(match);
        }

        public static void Lock() => Locked = true;
        public static void Unlock() => Locked = false;

        public static string GetRegistersCsv()
        {
            StringBuilder res = new StringBuilder();

            for (int i = 0; i < Registers.Count; i++)
            {
                var reg = Registers[i];
                res.Append($"{reg.Id},{reg.Id.ToLower()},${reg.Id},${reg.Id.ToLower()},${i}");
                if (i < Registers.Count - 1) res.Append(',');
            }
            
            return res.ToString();
        }
    }
}