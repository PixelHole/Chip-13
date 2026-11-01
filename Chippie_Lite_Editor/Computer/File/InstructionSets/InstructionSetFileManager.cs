using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.DefaultStates;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.File.InstructionSets;

public static class InstructionSetFileManager
{
    public static void LoadSets()
    {
        var instances = JsonFileUtility.ReadAndDeserialize<InstructionSetInstance[]>("Data/InstructionSets.json");

        foreach (var instance in instances)
        {
            var set = instance.ToSet();
            if (InstructionSetManager.HasSet(set)) continue;
            InstructionSetManager.LoadSet(set);
        }
    }
    public static void SaveSets()
    {
        var sets = InstructionSetManager.GetSets();
        var instances = new InstructionSetInstance[sets.Length];
        
        for (int i = 0; i < sets.Length; i++)
        {
            instances[i] = new InstructionSetInstance(sets[i]);
        }
        
        JsonFileUtility.SerializeAndWrite(instances, "Data/InstructionSets.json");
    }
}