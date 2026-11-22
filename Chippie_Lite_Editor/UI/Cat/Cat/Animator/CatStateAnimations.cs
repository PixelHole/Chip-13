namespace Chippie_Lite_WPF.UI.Cat.Cat.Animator;

public class CatStateAnimations
{
    public string StateName { get; set; }
    public CatAnimation[] Animations { get; set; } = [];
    
    public CatAnimation Current { get; private set; }
    
    public CatStateAnimations(string stateName, CatAnimation[] animations)
    {
        StateName = stateName;
        Animations = animations;
        Current = Animations[0];
    }

    
    public bool ChangeAnimation(bool skipExitTime = false)
    {
        Current = GetRandomNext();
        return true;
    }
    private CatAnimation GetRandomNext()
    {
        var chanceSum = Animations.Sum(anim => anim.Chance);
        Random random = new Random(DateTime.Now.Second);
        int chance = random.Next(1, chanceSum + 1);

        int sum = 0;
        foreach (var animation in Animations)
        {
            if (chance <= animation.Chance + sum) return animation;
            sum += animation.Chance;
        }

        return Current;
    }
}