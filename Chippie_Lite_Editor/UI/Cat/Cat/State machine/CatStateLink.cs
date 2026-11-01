namespace Chippie_Lite_WPF.UI.Cat;

public class CatStateLink
{
    public int Chance { get; set; }
    public CatState LinkedState { get; }


    public CatStateLink(int chance, CatState linkedState)
    {
        Chance = chance;
        LinkedState = linkedState;
    }

    public CatState? GetChanceState()
    {
        return GetChance() ? LinkedState : null;
    }
    public bool GetChance()
    {
        Random random = new Random();
        int chance = random.Next(0, 101);
        return chance <= Chance;
    }
}