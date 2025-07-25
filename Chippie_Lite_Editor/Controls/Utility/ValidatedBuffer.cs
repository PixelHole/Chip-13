using System.Timers;
using Timer = System.Timers.Timer;

namespace Chippie_Lite_WPF.Controls.Utility;

public class ValidatedBuffer<T>
{
    private T? Buffer { get; set; }
    public Predicate<T> Rule { get; private set; }
    private Timer SetTimer = new();
    private bool HasTimer = false;
    
    public ValidatedBuffer(Predicate<T> rule)
    {
        Rule = rule;
    }
    
    
    public bool Set(T value)
    {
        if (!Rule.Invoke(value)) return false;
        
        Buffer = value;
        return true;
    }
    public T Get()
    {
        return Buffer;
    }
}