namespace Chippie_Lite_WPF.Computer.Components;

public class Register
{
    private int _content;
        
    public string Name { get; }
    public string Id { get; }

    public int Content
    {
        get => _content;
        set
        {
            bool changed = value != _content;
            _content = value;
            if (changed) OnContentChanged?.Invoke(_content);
        }
    }

    public int DefaultValue { get; set; }

    public delegate void ContentChangeAction(int newValue);
    public event ContentChangeAction? OnContentChanged;
        
        
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
    public override int GetHashCode()
    {
        return HashCode.Combine(OnContentChanged, _content, Name, Id, DefaultValue);
    }
    private bool Equals(Register? register)
    {
        if (register == null) return false;
        return register.Name == Name;
    }
}