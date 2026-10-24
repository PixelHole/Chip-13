using System.Text;

namespace Chippie_Lite_WPF.Computer.Components
{
    public class RegisterBank : ChippieComponent
    {
        private List<Register> Registers { get; set; } =
        [
            ..new[]
            {
                new Register("Instruction Pointer", "IP"),
                new Register("A", "A"),
                new Register("B", "B"),
                new Register("C", "C"),
                new Register("D", "D"),
                new Register("E", "E"),
                new Register("F", "F"),
                new Register("High", "HG"),
                new Register("Low", "LW"),
                new Register("Stack Base", "SB"),
                new Register("Stack Pointer", "SP"),
                new Register("Input Buffer Count", "IB"),
            }
        ];
        public bool Locked { get; private set; }
    

        public bool AddRegister(Register register)
        {
            if (Locked) return false;
            if (GetRegister(register.Name) != null) return false;
            Registers.Add(register);
            return true;
        }
        public int AddRegisters(IEnumerable<Register> registers)
        {
            if (Locked) return 0;
            int count = 0;

            foreach (var register in registers)
            {
                if (AddRegister(register)) count++;
            }

            return count;
        }
        public int AddRegisters(params Register[] registers)
        {
            if (Locked) return 0;
            int count = 0;

            foreach (var register in registers)
            {
                if (AddRegister(register)) count++;
            }

            return count;
        }
        public bool RemoveRegister(Register register)
        {
            if (Locked) return false;
            return Registers.Remove(register);
        }
        public int RemoveRegisters(IEnumerable<Register> registers)
        {
            if (Locked) return 0;
        
            int count = 0;

            foreach (var register in registers)
            {
                if (RemoveRegister(register)) count++;
            }

            return count;
        }

        public void Clear()
        {
            Registers.Clear();
        }
        public void ResetRegisters()
        {
            foreach (var register in Registers)
            {
                register.ResetContent();
            }
        }

        public Register[] GetAllRegisters()
        {
            Register[] registers = new Register[Registers.Count];

            for (int i = 0; i < registers.Length; i++)
            {
                registers[i] = Registers[i];
            }
            
            return registers;
        }
        
        public Register GetRegister(int index)
        {
            return Registers[WrapIndex(index)];
        }
        public Register? GetRegister(string name)
        {
            return FindRegister(reg => string.Equals(reg.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }
        public Register? GetRegisterById(string id)
        {
            return FindRegister(reg => string.Equals(reg.Id, id, StringComparison.CurrentCultureIgnoreCase));
        }

        public List<Register> GetRegisters(IEnumerable<int> indices)
        {
            List<Register> registers = new List<Register>();
            if (Registers.Count == 0) return registers;

            foreach (var i in indices)
            {
                registers.Add(GetRegister(i));
            }

            return registers;
        }
        public List<Register> GetRegisters(IEnumerable<string> names)
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
        
        public Register? FindRegister(Predicate<Register> predicate)
        {
            return Registers.Find(predicate);
        }

        public int IndexOf(Register register)
        {
            return Registers.IndexOf(register);
        }
        public int IndexOf(string name)
        {
            Register? match = GetRegister(name);
            if (match == null) return -1;
            return IndexOf(match);
        }

        public void Lock() => Locked = true;
        public void Unlock() => Locked = false;

        private int WrapIndex(int index)
        {
            if (Registers.Count == 0) return 0;
            if (index >= Registers.Count) return index % Registers.Count;
            if (index < 0) return (Registers.Count - 1) -index % Registers.Count;
            return index;
        }

        public string GetRegistersCsv()
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