namespace Chippie_Lite_WPF.UI.Cat.Cat;

public struct CatState
{
    public string Name { get; private set; } = "State";
    private List<CatStateLink> Links { get; } = [];
    public bool CanTransition { get; set; } = true;
    public TimeSpan Duration { get; set; }


    public CatState(string name, TimeSpan duration)
    {
        Name = name;
        Duration = duration;
    }

    public void AddLink(CatState state, int chance)
    {
        Links.Add(new CatStateLink(chance, state));
    }

    public CatState Next()
    {
        if (!CanTransition) return this;
        
        foreach (var link in Links.Where(link => link.GetChance())) return link.LinkedState;

        return this;
    }
}