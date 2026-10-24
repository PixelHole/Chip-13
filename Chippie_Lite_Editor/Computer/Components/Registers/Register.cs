using System.Text.Json.Serialization;

namespace Chippie_Lite_WPF.Computer.Components;

public class Register
{
    private long _content;
    
    public string Name { get; }
    public string Id { get; }

    public long Content
    {
        get => _content;
        set
        {
            bool changed = value != _content;
            _content = LengthLimit == 64 ? value : value & ContentMask;
            if (changed) OnContentChanged?.Invoke(_content);
        }
    }
    public byte LengthLimit { get; set; }

    private long ContentMask
    {
        get
        {
            var mask = (long)Math.Pow(2, LengthLimit);
            return LengthLimit == 64 ? mask : mask - 1;
        }
    }

    public long DefaultValue { get; set; }

    public delegate void ContentChangeAction(long newValue);
    public event ContentChangeAction? OnContentChanged;
        
        
    [JsonConstructor]
    public Register(string name, string id, long content = 0, long defaultValue = 0, byte lengthLimit = 32)
    {
        Name = name;
        Id = id;
        Content = content;
        DefaultValue = defaultValue;
        LengthLimit = lengthLimit;
    }

    public void ResetContent()
    {
        Content = DefaultValue;
    }

    public override string ToString()
    {
        return $"{Name}({Id}) : {Content}";
    }

    public static implicit operator long(Register reg) => reg.Content;
    public static implicit operator int(Register reg) => (int)reg.Content;

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