namespace Chippie_Lite_WPF.Controls;

public class ValidatedBuffer<T>(Predicate<T> rule)
{
    private T? Buffer { get; set; }
    public Predicate<T> Rule { get; private set; } = rule;


    public bool Set(T value)
    {
        if (!Rule.Invoke(value)) return false;
        
        Buffer = value;
        return true;
    }
    public T? Get()
    {
        return Buffer ?? default(T);
    }
}