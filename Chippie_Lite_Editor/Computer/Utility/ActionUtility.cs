using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Arguments;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;

namespace Chippie_Lite_WPF.Computer.Utility;

public static class ActionUtility
{
    public static long InterpretRead(string inp, Instruction instruction)
    {
        if (inp.StartsWith('~')) return ~InterpretRead(inp[1..], instruction);
        if (inp.StartsWith('-')) return -InterpretRead(inp[1..], instruction);
        if (inp.StartsWith('+')) return long.Abs(InterpretRead(inp[1..], instruction));
        if (inp.StartsWith('@')) return ReadVariable(inp, instruction);
        if (inp.StartsWith('$')) return ReadRegister(inp, instruction);
        if (inp.StartsWith('^')) return InstructionArgument.GetNumber(ReadArgument(inp, instruction));
        if (inp.StartsWith('#')) return ReadMemory(inp, instruction);
        if (NumberUtility.TryParseNumber(inp, out var num)) return num;
        return -1;
    }
    // format : @VALUE
    private static long ReadVariable(string inp, Instruction instruction)
    {
        inp = inp[1..];
        if (!instruction.Variables.TryGetValue(inp, out var value))
            throw new InstructionInterpretationException(
                $"could not complete action from instruction : {instruction}\n" +
                $"variable with name {inp} was never created or assigned");  
        return value;
    }
    // format : ^VALUE
    private static InstructionArgument ReadArgument(string inp, Instruction instruction)
    {
        inp = inp[1..];
        long index = InterpretRead(inp, instruction);
        if (index == -1) throw new InstructionInterpretationException($"could not complete action from instruction : {instruction}\n" +
                                                                      $"could not get Argument with index {inp}");
        return instruction.Arguments[IndexUtility.WrapIndex((int)index, instruction.Arguments.Count)];
    }
    // format : $VALUE
    private static long ReadRegister(string inp, Instruction instruction)
    {
        return GetRegister(inp, instruction).Content;
    }
    // format : #INDEX|ADDRESS
    private static long ReadMemory(string inp, Instruction instruction)
    {
        var destination = GetMemory(inp, instruction);
        return destination.memory.Read(destination.address);
    }
    
    
    public static void InterpretWrite(string inp, long value, Instruction instruction)
    {
        if (inp.StartsWith('@')) WriteVariable(inp, value, instruction);
        else if (inp.StartsWith('$')) WriteRegister(inp, value, instruction);
        else if (inp.StartsWith('#')) WriteMemory(inp, value, instruction);
        else WriteVariable(inp, value, instruction);
    }
    private static void WriteVariable(string inp, long value, Instruction instruction)
    {
        inp = inp[1..];
        if (instruction.Variables.TryAdd(inp, value)) return;
        instruction.Variables[inp] = value;
    }
    private static void WriteRegister(string inp, long value, Instruction instruction)
    {
        var reg = GetRegister(inp, instruction);
        reg.Content = value;
    }
    private static void WriteMemory(string inp, long value, Instruction instruction)
    {
        var dest = GetMemory(inp, instruction);
        dest.memory.Write(dest.address, value);
    }

    public static Register GetRegister(string inp, Instruction instruction)
    {
        string name = inp.StartsWith('$') ? inp[1..] : inp;
        
        var reg = RegisterBank.GetRegisterById(name);
        if (reg != null) return reg;
        reg = RegisterBank.GetRegister(name);
        if (reg != null) return reg;

        if (name.StartsWith('^')) return InstructionArgument.GetRegister(instruction.Arguments[(int)InterpretRead(name[1..], instruction)]);
        
        long index = InterpretRead(name, instruction);
        if (index == -1) throw new InstructionInterpretationException($"could not complete action from instruction : {instruction}\n" +
                                                                      $"could not get register with index {inp}");
        reg = RegisterBank.GetRegister((int)index)!;
        if (reg == null) throw new InstructionInterpretationException($"could not complete action from instruction : {instruction}\n" +
                                                                      $"the register bank has no registers");
        return reg;
    }
    public static (Memory memory, int address) GetMemory(string inp, Instruction instruction)
    {
        if (inp.Length < 4 || !inp.Contains('|')) 
            throw new InstructionInterpretationException($"could not complete action from instruction : {instruction}\n" +
                                                         $"incorrect memory read format");
        
        long memIndex = InterpretRead(inp[1..][..inp.IndexOf('|')], instruction); 
        
        if (memIndex == -1) throw new InstructionInterpretationException(
            $"could not complete action from instruction : {instruction}\n" +
            $"The provided memory index is not a number");
        
        var memory = ChippieComponentManager.GetNthComponentOfType<Memory>((int)memIndex);
        if (memory == null) throw new InstructionInterpretationException(
            $"could not complete action from instruction : {instruction}\n" +
            $"could not get memory component with index {memIndex}");

        var address = InterpretRead(inp[inp.IndexOf('|')..], instruction);
        
        if(address == -1) throw new InstructionInterpretationException(
            $"could not complete action from instruction : {instruction}\n" +
            $"The provided Address is not a number");

        return (memory, (int)address);
    }
    
    internal static void CheckArgumentAndActionIndexCount(InstructionAction action, int expected)
    {
        if (action.Arguments.Length < expected) throw new NotEnoughActionIndicesException(action.Arguments.Length, expected);
    }
}