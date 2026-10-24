using System.Drawing;
using System.IO;
using System.Text.Json;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.Components;
using Chippie_Lite_WPF.Computer.File.DefaultStates;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Templates;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.File;

public static class SaveFileManager
{
     public static void SaveData(string code,  string path)
     {
          SaveInstance save = new SaveInstance(code, SerializeConfig());

          JsonFileUtility.SerializeAndWrite(save, path);
     }
     public static string LoadData(string path)
     {
          SaveInstance save = JsonFileUtility.ReadAndDeserialize<SaveInstance>(path);
          
          DeserializeConfig(save.Config);
          
          return save.Code;
     }
     public static void LoadDefaultData()
     {
          SaveInstance save = EmbeddedResourceUtility.DeserializeEmbeddedResource<SaveInstance>("Chippie_Lite_WPF.Default_instance.Default.json");
          
          DeserializeConfig(save.Config);
     }
     
     private static EnvironmentConfig SerializeConfig()
     {
          return new EnvironmentConfig(
               SerializeIO(), 
               SerializeMemoryModules(),
               SerializeInstructionSet(),
               SerializeRegisters(),
               SerializeColors());
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
               
               memoryModules.Add(new MemorySaveInstance(memory.Size,
                    memory.LengthLimit,
                    memory.ReadOnly,
                    memory.Permanent,
                    memory.BlockAddRange,
                    blocks.ToArray()));
          }

          return memoryModules.ToArray();
     }
     private static InstructionTemplate[] SerializeInstructionSet()
     {
          return InstructionSet.GetTemplates();
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
          
          foreach (var color in ColorLibrary.Colors)
          {
               colors.Add(new ColorSaveInstance(color));
          }

          return new ColorsSaveInstance(colors.ToArray());
     }
     
     private static void DeserializeConfig(EnvironmentConfig config)
     {
          DeserializeInstructionSet(config.InstructionSet);
          DeserializeMemory(config.MemoryModules);
          DeserializeRegisters(config.Registers);
          DeserializeColors(config.Colors);
          DeserializeIO(config.IOData);
     }
     private static void DeserializeInstructionSet(InstructionTemplate[] instructionSet)
     {
          InstructionSet.SetTemplates(instructionSet);
     }
     private static void DeserializeMemory(MemorySaveInstance[] memoryModules)
     {
          ChippieComponentManager.RemoveAllComponentsOfType<Memory>();
          foreach (var module in memoryModules)
          {
               var memory = module.ToMemory();
               ChippieComponentManager.AddComponent(memory);
          }
     }
     private static void DeserializeRegisters(RegisterSaveInstance[] registers)
     {
          RegisterBank.Clear();
          foreach (var instance in registers)
          {
               RegisterBank.AddRegister(instance.ToRegister());
          }
     }
     private static void DeserializeColors(ColorsSaveInstance colors)
     {
          ColorLibrary.Clear();
          foreach (var instance in colors.Colors)
          {
               ColorLibrary.AddColor(instance.ToColor());
          }
     }
     private static void DeserializeIO(IOSaveInstance instance)
     {
          IOInterface.DisplaySize = instance.DisplaySize;
     }
}