namespace Chippie_Lite_WPF.UI.Cat;

public class CatState
{
    private List<CatStateLink> Links { get; } = [];
    

    public void AddLink(CatState state, int chance)
    {
        Links.Add(new CatStateLink(chance, state));
    }

    public CatState Next()
    {
        foreach (var link in Links)
        {
            if (link.GetChance()) return link.LinkedState;
        }

        return this;
    }
}