using Chippie_Lite.Computer.Components.InstructionBanks;
using Chippie_Lite.Computer.Components.Processing;
using Chippie_Lite.Computer.Components.Registers;
using Chippie_Lite.Computer.Instructions;
using Chippie_Lite.Internal;

namespace Chippie_Lite.Computer;

public static class Chippie
{
    public static bool IsRunning { get; private set; }
    public static bool SingleStep { get; set; }
    public static Register InstructionPointer { get; private set; }
    private static Thread ExecutionThread { get; set; } = new Thread(ExecutionLoop);
    private static bool CanGoToNextStep { get; set; } = false;


    public static void Initialize()
    {
        InstructionPointer = RegisterBank.GetRegister("Instruction Pointer")!;
    }
    
    public static string CompileAndLoadAssembly(string raw)
    {
        if (IsRunning) return "cannot compile while running";
        List<Instruction> instructions = new List<Instruction>();
        try
        {
            instructions = AssemblyTranslator.TranslateMultiline(raw);
            InstructionBank.ClearInstructions();
            InstructionBank.AddInstructions(instructions);
        }
        catch (Exception)
        {
            return "error occured while compiling";
        }

        return "Successfully compiled";
    }
    public static string Run()
    {
        if (IsRunning) return "Already Running";
        
        IsRunning = true;
        ExecutionThread.Start();
        return "Started running";
    }

    public static void NextStep()
    {
        CanGoToNextStep = true;
    }
    
    private static void ExecutionLoop()
    {
        while (true)
        {
            if (SingleStep && !CanGoToNextStep) continue;
            ExecutionStep();
            if (InstructionPointer.Content >= InstructionBank.Count || InstructionPointer.Content < 0) break;
        }

        IsRunning = false;
    }
    private static void ExecutionStep()
    {
        InstructionProcessor.ExecuteNextInstruction(InstructionPointer);
        if (SingleStep) CanGoToNextStep = false;
    }
}