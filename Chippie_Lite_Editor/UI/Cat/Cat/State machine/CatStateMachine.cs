using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Cat;

public class CatStateMachine
{
    public static CatStateMachine LoadFromResource(string name)
    {
        var raw = EmbeddedResourceUtility.ReadEmbeddedResource(name);
        
        
        
        return new CatStateMachine();
    }
}