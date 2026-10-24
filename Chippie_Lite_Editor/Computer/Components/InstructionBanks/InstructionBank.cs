using Chippie_Lite_WPF.Computer.Instructions;

namespace Chippie_Lite_WPF.Computer.Components;

public class InstructionBank : ChippieComponent
{
    public List<Instruction> Instructions { get; private set; } = new List<Instruction>();
    public long Count => Instructions.Count;
    
    
    public void AddInstruction(Instruction ins) => Instructions.Add(ins);
    public void AddInstructions(IEnumerable<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            AddInstruction(instruction);
        }
    }

    public bool RemoveInstruction(Instruction ins) => Instructions.Remove(ins);
    public int RemoveInstructions(IEnumerable<Instruction> instructions)
    {
        int count = 0;
        foreach (var instruction in instructions)
        {
            if (RemoveInstruction(instruction)) count++;
        }
        return count;
    }

    public void ReplaceInstruction(int index, Instruction newIns)
    {
        Instructions[WrapIndex(index)] = newIns;
    }

    public void ClearInstructions()
    {
        Instructions.Clear();
    }
    
    public Instruction GetInstruction(int index) => Instructions[WrapIndex(index)];

    private int WrapIndex(int index)
    {
        if (Count == 0) return 0;
        if (index >= Instructions.Count) return index % Instructions.Count;
        if (index < 0) return (Instructions.Count - 1) - (-index % Instructions.Count);
        return index;
    }
}