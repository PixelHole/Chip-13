using System.IO;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File.DefaultStates;

namespace Chippie_Lite_WPF.Computer.File;

public static class SaveFileManager
{
     public static void SaveData(string code,  string path)
     {
          SaveInstance save = new SaveInstance(code, SerializeRegisters(), SerializeMemory());

          string json = SaveInstance.Serialize(save);

          StreamWriter writer = new StreamWriter(path);
          
          writer.Write(json);
          
          writer.Flush();
          writer.Close();
     }
     public static string LoadData(string path)
     {
          StreamReader reader = new StreamReader(path);
          
          string json = reader.ReadToEnd();
          
          reader.Close();
          
          SaveInstance save = SaveInstance.Deserialize(json);
          
          DeserializeRegisters(save.Registers);
          DeserializeMemory(save.MemoryBlocks);
          
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
               register.DefaultValue = instance.DefaultValue;
          }
     }
     private static MemoryBlockSaveInstance[] SerializeMemory()
     {
          List<MemoryBlockSaveInstance> blocks = [];
          
          foreach (var block in Memory.GetAllBlocks())
          {
               blocks.Add(new MemoryBlockSaveInstance(block));
          }

          return blocks.ToArray();
     }
     private static void DeserializeMemory(MemoryBlockSaveInstance[] memory)
     {
          foreach (var block in memory)
          {
               Memory.AddBlock(block.ToBlock());
          }
     }
}