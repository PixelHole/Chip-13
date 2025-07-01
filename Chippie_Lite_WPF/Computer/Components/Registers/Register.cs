namespace Chippie_Lite_WPF.Computer.Components
{
    public class Register
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public int Content { get; private set; }
        public int DefaultValue { get; private set; }


        public Register(string name, string id, int content = 0, int defaultValue = 0)
        {
            Name = name;
            Id = id;
            SetContent(content);
            DefaultValue = defaultValue;
        }

        public void ResetContent()
        {
            SetContent(DefaultValue);
        }
        
        public void SetContent(int content)
        {
            Content = content;
        }
        public void SetContent(Register register)
        {
            SetContent(register.Content);
        }
        public void AddToContent(int amount)
        {
            if (amount < 0)
            {
                ReduceFromContent(-amount);
                return;
            }
        
            if (int.MaxValue - amount < Content)
            {
                Content = int.MinValue + (int.MaxValue - amount);
                return;
            }

            Content += amount;
        }
        public void AddToContent(Register register)
        {
            AddToContent(register.Content);
        }
        public void ReduceFromContent(int amount)
        {
            if (amount < 0)
            {
                AddToContent(-amount);
                return;
            }
        
            if (int.MinValue + amount > Content)
            {
                Content = int.MaxValue - (int.MinValue + amount);
                return;
            }

            Content -= amount;
        }
        public void ReduceFromContent(Register register)
        {
            ReduceFromContent(register.Content);
        }


        public override string ToString()
        {
            return $"{Name}({Id}) : {Content}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Register reg) return Equals(reg);
            return base.Equals(obj);
        }
        public bool Equals(Register? register)
        {
            if (register == null) return false;
            return register.Name == Name;
        }
    }
}