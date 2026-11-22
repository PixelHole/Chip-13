using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.Computer.File;

public static class SaveFileManager
{
     
     
     public static void SaveToFile(string path, SaveOptions? options = null)
     {
          options ??= SaveOptions.FullOptions;
          
          SaveInstance save = new SaveInstance(MainWindow.MainWindowIns.GetCode(), SerializeConfig(options));

          JsonFileUtility.SerializeAndWrite(save, path);
     }
     public static string LoadFromFile(string path, SaveOptions? options = null)
     {
          options ??= SaveOptions.FullOptions;
          
          SaveInstance save = JsonFileUtility.ReadAndDeserialize<SaveInstance>(path);
          
          DeserializeConfig(save.Config, options);
          
          return save.Code;
     }
     public static void LoadDefaultData()
     {
          SaveInstance save = EmbeddedResourceUtility.DeserializeEmbeddedResource<SaveInstance>("Chippie_Lite_WPF.Resources.Data.Default.json");
          
          DeserializeConfig(save.Config, SaveOptions.FullOptions);
     }
     
     private static EnvironmentConfig SerializeConfig(SaveOptions options)
     {
          return new EnvironmentConfig(
               options.HasFlag(SaveOptions.SaveTargets.IO) ? SerializeIO() : null, 
               options.HasFlag(SaveOptions.SaveTargets.Memory) ? SerializeMemoryModules() : null,
               options.HasFlag(SaveOptions.SaveTargets.InstructionSet) ? SerializeInstructionSet() : null,
               options.HasFlag(SaveOptions.SaveTargets.Registers) ? SerializeRegisters() : null,
               options.HasFlag(SaveOptions.SaveTargets.Colors) ? SerializeColors() : null);
     }
     private static IOSaveInstance SerializeIO()
     {
          return new IOSaveInstance(IOInterface.DisplaySize);
     }
     private static MemorySaveInstance[] SerializeMemoryModules()
     {
          List<MemorySaveInstance> memoryModules = [];

          foreach (var memory in ChippieComponentManager.GetComponentsOfType<Memory>())
          {
               List<MemoryBlockSaveInstance> blocks = [];
          
               foreach (var block in memory.GetAllInitialBlocks())
               {
                    blocks.Add(new MemoryBlockSaveInstance(block));
               }
               
               memoryModules.Add(new MemorySaveInstance(
                    memory.Name,
                    memory.Size,
                    memory.LengthLimit,
                    memory.ReadOnly,
                    memory.Permanent,
                    memory.BlockAddRange,
                    blocks.ToArray()));
          }

          return memoryModules.ToArray();
     }
     private static InstructionSetInstance SerializeInstructionSet()
     {
          var activeSet = InstructionSetManager.ActiveInstructionSet;
          return new InstructionSetInstance(activeSet.Name, activeSet.GetTemplates());
     }
     private static RegisterSaveInstance[] SerializeRegisters()
     {
          List<RegisterSaveInstance> registers = [];
          
          registers.AddRange(RegisterBank.GetAllRegisters().Select(register => new RegisterSaveInstance(register)));

          return registers.ToArray();
     }
     private static ColorsSaveInstance SerializeColors()
     {
          List<ColorSaveInstance> colors = [];
          
          foreach (var color in ColorLibrary.GetColors())
          {
               colors.Add(new ColorSaveInstance(color));
          }

          return new ColorsSaveInstance(colors.ToArray());
     }
     
     private static void DeserializeConfig(EnvironmentConfig config, SaveOptions options)
     {
          if (options.HasFlag(SaveOptions.SaveTargets.InstructionSet)) DeserializeInstructionSet(config.InstructionSet);
          if (options.HasFlag(SaveOptions.SaveTargets.Memory))DeserializeMemory(config.MemoryModules);
          if (options.HasFlag(SaveOptions.SaveTargets.Registers)) DeserializeRegisters(config.Registers);
          if (options.HasFlag(SaveOptions.SaveTargets.Colors)) DeserializeColors(config.Colors);
          if (options.HasFlag(SaveOptions.SaveTargets.IO)) DeserializeIO(config.IOData);
     }
     private static void DeserializeInstructionSet(InstructionSetInstance? instructionSet)
     {
          if (instructionSet == null) return;
          var set = instructionSet.ToSet();
          if (!InstructionSetManager.HasSet(set)) InstructionSetManager.LoadSet(set);
          InstructionSetManager.ActiveSetIndex = InstructionSetManager.IndexOf(set);
     }
     private static void DeserializeMemory(MemorySaveInstance[]? memoryModules)
     {
          if (memoryModules == null) return;
          ChippieComponentManager.RemoveAllComponentsOfType<Memory>();
          foreach (var module in memoryModules)
          {
               var memory = module.ToMemory();
               ChippieComponentManager.AddComponent(memory);
          }
     }
     private static void DeserializeRegisters(RegisterSaveInstance[]? registers)
     {
          if (registers == null) return;
          RegisterBank.Clear();
          foreach (var instance in registers)
          {
               RegisterBank.AddRegister(instance.ToRegister());
          }
     }
     private static void DeserializeColors(ColorsSaveInstance? colors)
     {
          if (colors == null) return;
          ColorLibrary.Clear();
          foreach (var instance in colors.Colors)
          {
               ColorLibrary.AddColor(instance.ToColor());
          }
     }
     private static void DeserializeIO(IOSaveInstance? instance)
     {
          if (instance == null) return;
          IOInterface.DisplaySize = instance.Value.DisplaySize;
     }
}