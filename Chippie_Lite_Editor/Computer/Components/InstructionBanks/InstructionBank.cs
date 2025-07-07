using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components;

public static class InstructionBank
{
    public static List<Instruction> Instructions { get; private set; } = new List<Instruction>();
    public static int Count => Instructions.Count;
    
    
    public static void AddInstruction(Instruction ins) => Instructions.Add(ins);
    public static void AddInstructions(IEnumerable<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            AddInstruction(instruction);
        }
    }

    public static bool RemoveInstruction(Instruction ins) => Instructions.Remove(ins);
    public static int RemoveInstructions(IEnumerable<Instruction> instructions)
    {
        int count = 0;
        foreach (var instruction in instructions)
        {
            if (RemoveInstruction(instruction)) count++;
        }
        return count;
    }

    public static void ReplaceInstruction(int index, Instruction newIns)
    {
        Instructions[WrapIndex(index)] = newIns;
    }

    public static void ClearInstructions()
    {
        Instructions.Clear();
    }
    
    public static Instruction GetInstruction(int index) => Instructions[WrapIndex(index)];

    private static int WrapIndex(int index)
    {
        if (Count == 0) return 0;
        if (index >= Instructions.Count) return index % Instructions.Count;
        if (index < 0) return (Instructions.Count - 1) - (-index % Instructions.Count);
        return index;
    }
}