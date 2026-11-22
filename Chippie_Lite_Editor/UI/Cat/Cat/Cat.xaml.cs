using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Cat.Cat.Animator;

namespace Chippie_Lite_WPF.UI.Cat.Cat;

public partial class Cat : UserControl
{
    public CatControl Control { get; private set; }
    public CatAnimator Animator { get; private set; }
    public CatContainer Container { get; private set; }
    
    
    public Cat(string name, CatContainer container)
    {
        InitializeComponent();
        UpdateSprite();
        Container = container;
        CreateComponents();
        RandomizePosition();
        SetName(name);
    }

    private void SetName(string name)
    {
        WPFUtility.SetContent(Label, name);
    }
    
    private void UpdateSprite()
    {
        Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
    }
    private void CreateComponents()
    {
        Control = new CatControl(this);
        Animator = new CatAnimator(this, Sprite);
    }
    private void RandomizePosition()
    {
        Random random = new Random(DateTime.Now.Second);
        Control.SetPosition(random.Next(0, (int)Container.MaxWidth - 64));
    }

    public void StopProcesses()
    {
        Control.Active = false;
        Animator.StopTimer();
    }
}