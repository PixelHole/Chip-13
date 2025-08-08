using System.IO;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.DefaultStates;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.File;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.Computer.File;

public static class SaveFileManager
{
     public static void SaveData(string code,  string path)
     {
          SaveInstance save = new SaveInstance(code, SerializeRegisters(), SerializeMemory(), SerializeConfig());

          JsonFileUtility.SerializeAndWrite(save, path);
     }
     public static string LoadData(string path)
     {
          SaveInstance save = JsonFileUtility.ReadAndDeserialize<SaveInstance>(path);
          
          DeserializeRegisters(save.Registers);
          DeserializeMemory(save.MemoryBlocks);
          DeserializeConfig(save.Config);
          
          return save.Code;
     }

     private static RegisterSaveInstance[] SerializeRegisters()
     {
          List<RegisterSaveInstance> registers = [];
          
          foreach (var register in RegisterBank.GetAllRegisters())
          {
               if (register.DefaultValue == 0) continue;
               registers.Add(new RegisterSaveInstance(register));
          }

          return registers.ToArray();
     }
     private static void DeserializeRegisters(RegisterSaveInstance[] registers)
     {
          foreach (var instance in registers)
          {
               var register = RegisterBank.GetRegister(instance.Name);
               if (register == null) throw new RegisterNotFoundException(0, instance.Name);
               register.DefaultValue = instance.DefaultValue;
          }
     }
     private static MemoryBlockSaveInstance[] SerializeMemory()
     {
          List<MemoryBlockSaveInstance> blocks = [];
          
          foreach (var block in Memory.GetAllInitialBlocks())
          {
               blocks.Add(new MemoryBlockSaveInstance(block));
          }

          return blocks.ToArray();
     }
     private static void DeserializeMemory(MemoryBlockSaveInstance[] memory)
     {
          MemoryBlock[] blocks = new MemoryBlock[memory.Length];

          for (int i = 0; i < memory.Length; i++)
          {
               blocks[i] = memory[i].ToBlock();
          }

          Memory.LoadInitialBlocks(blocks);
     }
     private static EnvironmentConfig SerializeConfig()
     {
          return new EnvironmentConfig(IOInterface.DisplaySize, Memory.Size, [], []);
     }
     private static void DeserializeConfig(EnvironmentConfig config)
     {
          IOInterface.DisplaySize = config.DisplaySize;
          Memory.Size = config.MemorySize;
     }
}