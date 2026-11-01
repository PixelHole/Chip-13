namespace Chippie_Lite_WPF.UI.Cat;

public class CatControl
{
    private CatStateMachine StateMachine { get; } = CatStateMachine.LoadFromResource("Chippie_Lite_WPF.Resources.Data.CatStates.txt");
}