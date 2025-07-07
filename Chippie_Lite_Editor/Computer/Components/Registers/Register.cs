namespace Chippie_Lite_WPF.Computer.Components
{
    public class Register
    {
        private int _content;
        
        public string Name { get; private set; }
        public string Id { get; private set; }

        public int Content
        {
            get => _content;
            set
            {
                _content = value;
                OnContentChanged?.Invoke(_content);
            }
        }

        public int DefaultValue { get; private set; }

        public delegate void ContentChangeAction(int newValue);
        public event ContentChangeAction OnContentChanged;
        
        
        public Register(string name, string id, int content = 0, int defaultValue = 0)
        {
            Name = name;
            Id = id;
            Content = content;
            DefaultValue = defaultValue;
        }

        public void ResetContent()
        {
            Content = DefaultValue;
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