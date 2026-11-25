using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;

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
    
    public static void ExportToFile(InstructionSet set, string path)
    {
        try
        {
            var serialized = SaveFileManager.GetSaveInstance(set, SaveOptions.InstructionSetOptions);
            JsonFileUtility.SerializeAndWrite(serialized, path);
        }
        catch (Exception e)
        {
            ErrorBox.Pop("Error!", e.Message);
        }
    }
    public static void ImportFromFile(string path)
    {
        try
        {
            var instance = JsonFileUtility.ReadAndDeserialize<SaveInstance>(path);
            var set = instance.Config.InstructionSet?.ToSet() ?? throw new NullReferenceException();
            InstructionSetManager.LoadSet(set);
            InstructionSetManager.ActiveSetIndex = InstructionSetManager.IndexOf(set);
        }
        catch (Exception e)
        {
            ErrorBox.Pop("Error!", e.Message);
        }
    }
}