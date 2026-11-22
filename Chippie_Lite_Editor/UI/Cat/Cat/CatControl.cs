using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Cat.Cat;

public class CatControl
{
    private int direction = 0;
    private readonly Cat View;
    private Canvas? Parent => View.Parent as Canvas;
    
    public CatStateMachine StateMachine { get; } = CatStateMachine.LoadFromResource("Chippie_Lite_WPF.Resources.Data.CatStates.json");

    public double Position { get; set; } = 0;
    
    public bool Active { get; set; } = true;

    private int WalkDirection
    {
        get => direction;
        set
        {
            if (direction == value) return;
            direction = value;
            OnDirectionChanged?.Invoke(direction);
        }
    }

    public delegate void DirectionChangedAction(int direction);
    public event DirectionChangedAction? OnDirectionChanged;
    

    public CatControl(Cat view)
    {
        View = view;
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        StateMachine.OnStateChanged += StateMachineOnOnStateChanged;
    }
    private void StateMachineOnOnStateChanged(CatState previous, CatState current)
    {
        switch (current.Name)
        {
            case "Sleeping":
                current.Duration = TimeSpan.FromSeconds(Random.Shared.Next(5, 15));
                break;
            case "Sitting":
                current.Duration = TimeSpan.FromSeconds(Random.Shared.Next(3, 10));
                break;
            case "Walking":
                current.Duration = TimeSpan.FromSeconds(Random.Shared.Next(2, 5));
                break;
        }
    }

    public void CatBrain()
    {
        if (!Active) return;
        
        switch (StateMachine.Current.Name)
        {
            case "Sitting":
                SittingBehavior();
                break;
            case "Walking":
                WalkingBehavior();
                break;
            case "Sleeping":
                SleepingBehavior();
                break;
        }
    }

    private void SittingBehavior()
    {
        WalkDirection = 0;
    }
    private void WalkingBehavior()
    {
        if (WalkDirection == 0) WalkDirection = Random.Shared.Next(0, 101) > 50 ? 1 : -1;
        
        if (Position < 0)
        {
            WalkDirection = 1;
            Position = 0;
        }
        else if (Position > View.Container.MaxWidth)
        {
            WalkDirection = -1;
            Position = View.Container.MaxWidth - 64;
        }

        Console.WriteLine(Position);
        
        SetPosition(Position + WalkDirection * 1.5d);
    }
    private void SleepingBehavior()
    {
        WalkDirection = 0;
    }

    public void SetPosition(double position)
    {
        Position = position;
        WPFUtility.DoUiAction(View, () => Canvas.SetLeft(View, Position));
    }
}