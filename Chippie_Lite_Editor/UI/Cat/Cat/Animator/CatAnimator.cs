using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AnimatedImage.Wpf;
using Chippie_Lite_WPF.Computer.Utility;
using Timer = System.Timers.Timer;

namespace Chippie_Lite_WPF.UI.Cat.Cat.Animator;

public class CatAnimator
{
    public Image DisplayImage { get; }
    public Cat View { get; }

    public CatStateAnimations[] Animations =
    [
        new("Sitting", [
            new CatAnimation("Idle", 1000d, 1d, 4, new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Cat/sit idle.gif", UriKind.Absolute))),
            new CatAnimation("Blink", 250d, 0.5d, 1, new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Cat/sit idle blink.gif", UriKind.Absolute)))
        ]),
        new("Sleeping", [
            new CatAnimation("Sleep", 1000d, 0.1d, 1, new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Cat/sleep.gif", UriKind.Absolute)))
        ]),
        new("Walking", [
            new CatAnimation("Walk", 1000d, 0.9d, 1, new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Cat/Walk.gif", UriKind.Absolute))),
        ]),
    ];
    private CatStateAnimations? Current { get; set; }

    private Timer? AnimationTimer { get; set; }


    public CatAnimator(Cat view, Image displayImage)
    {
        View = view;
        DisplayImage = displayImage;
        ConnectEvents();
        UpdateCurrentAnimation();
        StartTimer();
    }
    
    
    private void ConnectEvents()
    {
        View.Control.StateMachine.OnStateChanged += StateMachineOnOnStateChanged;
        View.Control.OnDirectionChanged += ControlOnOnDirectionChanged;
    }
    private void ControlOnOnDirectionChanged(int direction)
    {
        if (direction == 0) return;
        WPFUtility.DoUiAction(DisplayImage, () => DisplayImage.RenderTransform = new ScaleTransform(-direction, 1));
    }
    private void StateMachineOnOnStateChanged(CatState previous, CatState current)
    {
        try
        {
            var next = Animations.First(animations => animations.StateName == current.Name);
            Current = next;
        }
        catch (Exception)
        {
            // ignored
        }
        
        UpdateAnimation();
        StartTimer();
    }

    private void UpdateCurrentAnimation()
    {
        try
        {
            var next = Animations.First(animations => animations.StateName == View.Control.StateMachine.Current.Name);
            Current = next;
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void StartTimer()
    {
        AnimationTimer?.Stop();
        AnimationTimer?.Dispose();
        AnimationTimer = new Timer(Current!.Current.Duration);
        AnimationTimer.Elapsed += (sender, args) => TimerElapsed();
        AnimationTimer.Start();
    }
    private void TimerElapsed()
    {
        if (AnimationTimer == null) return;
        AnimationTimer.Stop();
        UpdateAnimation();
        AnimationTimer.Interval = Current!.Current.Duration;
        AnimationTimer?.Start();
    }
    public void StopTimer()
    {
        if (AnimationTimer == null) return;
        AnimationTimer.Stop();
        AnimationTimer.Close();
        AnimationTimer.Dispose();
    }
    
    private void UpdateAnimation(bool forceChange = false)
    {
        WPFUtility.DoUiAction(View, () => UpdateAnimationAction(forceChange));
    }
    private void UpdateAnimationAction(bool forceChange = false)
    {
        Current!.ChangeAnimation();
        var animation = Current.Current;
        ImageBehavior.SetAnimatedSource(DisplayImage, animation.Source);
        ImageBehavior.SetAnimationSpeedRatio(DisplayImage, animation.PlaybackSpeed);
    }
}