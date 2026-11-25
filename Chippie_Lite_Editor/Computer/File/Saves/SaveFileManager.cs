using System.ComponentModel;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.Components;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Windows;
using Microsoft.Win32;

namespace Chippie_Lite_WPF.Computer.File;

public static class SaveFileManager
{
     public delegate void FileSavedAction(SaveInstance file, string path, SaveOptions options);
     public delegate void FileLoadedAction(SaveInstance file, string path, SaveOptions options);

     public static event FileSavedAction? OnFileSaved;
     public static event FileLoadedAction? OnFileLoaded;
     
     
     private static void LaunchLoadWindow()
     {
          OpenFileDialog fileDialog = new OpenFileDialog()
          {
               Multiselect = false,
               Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
          };
        
          fileDialog.FileOk += LoadDialogOk;
        
          fileDialog.ShowDialog();
     }
     private static void LaunchSaveWindow()
     {
          SaveFileDialog fileDialog = new SaveFileDialog()
          {
               CreatePrompt = false,
               OverwritePrompt = true,
               Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
          };
        
          fileDialog.FileOk += SaveDialogOk;
        
          fileDialog.ShowDialog();
     }
     private static void LoadDialogOk(object? sender, CancelEventArgs e)
     {
          var fileDialog = sender as OpenFileDialog;
          fileDialog!.FileOk -= LoadDialogOk;
        
          string path = fileDialog.FileName;

          LoadFromFile(path);
     }
     private static void SaveDialogOk(object? sender, CancelEventArgs e)
     {
          var fileDialog = sender as SaveFileDialog;
          fileDialog!.FileOk -= SaveDialogOk;

          string path = fileDialog.FileName;

          SaveToFile(path);
     }
     
     public static void SaveToFile(string path, SaveOptions? options = null)
     {
          options ??= SaveOptions.FullOptions;

          if (string.IsNullOrEmpty(path))
          {
               LaunchSaveWindow();
               return;
          }
          
          SaveInstance save = GetSaveInstance(InstructionSetManager.ActiveInstructionSet, options);

          JsonFileUtility.SerializeAndWrite(save, path);
          
          OnFileSaved?.Invoke(save, path, options);
     }
     public static void LoadFromFile(string path, SaveOptions? options = null)
     {
          options ??= SaveOptions.FullOptions;

          if (string.IsNullOrEmpty(path))
          {
               LaunchLoadWindow();
               return;
          }
          
          SaveInstance save = JsonFileUtility.ReadAndDeserialize<SaveInstance>(path);
          
          DeserializeConfig(save.Config, options);
          
          MainWindow.MainWindowIns.DevArea.SetInputCode(save.Code);
          
          OnFileLoaded?.Invoke(save, path, options);
     }
     public static void LoadDefaultData()
     {
          SaveInstance save = EmbeddedResourceUtility.DeserializeEmbeddedResource<SaveInstance>("Chippie_Lite_WPF.Resources.Data.Default.json");
          
          DeserializeConfig(save.Config, SaveOptions.FullOptions);
     }

     public static SaveInstance GetSaveInstance(InstructionSet set, SaveOptions options)
     {
          return new SaveInstance(MainWindow.MainWindowIns.GetCode(), SerializeConfig(set, options));
     }
     
     private static EnvironmentConfig SerializeConfig(InstructionSet set, SaveOptions options)
     {
          return new EnvironmentConfig(
               options.HasFlag(SaveOptions.SaveTargets.IO) ? SerializeIO() : null, 
               options.HasFlag(SaveOptions.SaveTargets.Memory) ? SerializeMemoryModules() : null,
               options.HasFlag(SaveOptions.SaveTargets.InstructionSet) ? SerializeInstructionSet(set) : null,
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
     private static InstructionSetInstance SerializeInstructionSet(InstructionSet set)
     {
          return new InstructionSetInstance(set.Name, set.GetTemplates());
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