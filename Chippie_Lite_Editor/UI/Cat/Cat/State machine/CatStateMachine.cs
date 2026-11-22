using System.Timers;
using Timer = System.Timers.Timer;

namespace Chippie_Lite_WPF.UI.Cat.Cat;

public class CatStateMachine
{
    public List<CatState> States { get; set; }= [];
    public CatState Current { get; private set; }

    private Timer StateTimer { get; set; } = new Timer(1);
    
    public delegate void StateChangedAction(CatState previous, CatState current);
    public event StateChangedAction? OnStateChanged;


    public CatStateMachine()
    {
        CatState sitting = new("Sitting", TimeSpan.FromSeconds(5)),
            walking = new("Walking", TimeSpan.FromSeconds(3)),
            Sleeping = new("Sleeping", TimeSpan.FromSeconds(8));
        
        sitting.AddLink(walking, 40);
        walking.AddLink(sitting, 100);
        sitting.AddLink(Sleeping, 20);
        Sleeping.AddLink(sitting, 100);
        
        States.AddRange([sitting, walking, Sleeping]);

        Current = sitting;
        
        Start();
    }
    public static CatStateMachine LoadFromResource(string name)
    {
        CatStateMachine stateMachine = new CatStateMachine();
        
        return stateMachine;
    }
    
    public void StopTimer()
    {
        StateTimer.Stop();
        StateTimer.Close();
        StateTimer.Dispose();
    }

    private void Start()
    {
        StateTimer.Stop();
        StateTimer.Close();
        StateTimer.Dispose();

        StateTimer = new Timer(Current.Duration);
        StateTimer.Elapsed += StateTimerOnElapsed;
        StateTimer.Start();
    }
    private void StateTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        StateTimer.Stop();
        GoNext();
        StateTimer.Interval = Current.Duration.TotalMilliseconds;
        StateTimer.Start();
    }

    private void GoNext()
    {
        var previous = Current;
        Current = Current.Next();
        if (Current.Name == previous.Name) return;
        OnStateChanged?.Invoke(previous, Current);
    }
}