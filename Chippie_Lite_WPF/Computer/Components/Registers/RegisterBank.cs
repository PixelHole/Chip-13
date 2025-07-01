namespace Chippie_Lite.Computer.Components.Registers
{
    public static class RegisterBank
    {
        private static List<Register> Registers { get; set; } = new List<Register>(new []
        {
            new Register("Instruction Pointer", "ID"),
            new Register("A", "A"),
            new Register("B", "B"),
            new Register("C", "C"),
        });
        public static bool Locked { get; private set; } = false;
    

        public static bool AddRegister(Register register)
        {
            if (Locked) return false;
            if (GetRegister(register.Name) != null) return false;
            Registers.Add(register);
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
            return Registers.Remove(register);
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
        
        public static Register? GetRegister(int index)
        {
            if (Registers.Count == 0) return null;
        
            return Registers[WrapIndex(index)];
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
            List<Register> registers = new List<Register>();
            if (Registers.Count == 0) return registers;

            foreach (var i in indices)
            {
                registers.Add(GetRegister(i));
            }

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
            return Registers.IndexOf(register);
        }
        public static int IndexOf(string name)
        {
            Register? match = GetRegister(name);
            if (match == null) return -1;
            return IndexOf(match);
        }

        public static void Lock() => Locked = true;
        public static void Unlock() => Locked = false;

        private static int WrapIndex(int index)
        {
            if (index >= Registers.Count) return index % Registers.Count;
            if (index < 0) return (Registers.Count - 1) -index % Registers.Count;
            return index;
        }
    }
}