using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Templates;
using Chippie_Lite_WPF.Computer.Internal;

namespace Chippie_Lite_WPF.Computer;

public static class Chippie
{
    public static bool IsRunning { get; private set; }
    public static bool CanRun { get; private set; } = true;
    public static bool SingleStep { get; set; }
    public static Register InstructionPointer { get; private set; }
    private static Thread ExecutionThread { get; set; }
    private static bool CanGoToNextStep { get; set; } = false;


    public delegate void CompileStartAction();
    public static event CompileStartAction OnCompileStarted;
    
    public delegate void CompileEndAction();
    public static event CompileEndAction OnCompileEnded;
    
    public delegate void RunFinishedAction();
    public static event RunFinishedAction OnRunFinished;

    public delegate void RunStartAction();
    public static event RunStartAction OnRunStarted;

    public static void Initialize()
    {
        InstructionPointer = RegisterBank.GetRegister("Instruction Pointer")!;
        InstructionSet.LoadSet(InstructionSet.SavePath);
    }
    
    public static int RunRawAssembly(string raw)
    {
        if (IsRunning) return -1;
        if (!FullFlush()) return 1;
        if (!CompileAndLoadAssembly(raw)) return 2;
        if (!Run()) return 3;
        return 0;
    }
    public static int Restart()
    {
        while (IsRunning)
        {
            HaltOperation();
        }
        
        if (!FlushRuntimeStorage()) return 1;
        if (!Run()) return 2;
        return 0;
    }
    public static int HaltOperation()
    {
        CanRun = false;
        return 0;
    }
    
    private static bool CompileAndLoadAssembly(string raw)
    {
        if (IsRunning) return false;
        List<Instruction> instructions = new List<Instruction>();
        
        OnCompileStarted?.Invoke();
        
        instructions = AssemblyTranslator.ParseScript(raw);
        InstructionBank.ClearInstructions();
        InstructionBank.AddInstructions(instructions);

        OnCompileEnded?.Invoke();
        
        return true;
    }
    private static bool FullFlush()
    {
        if (IsRunning) return false;
        InstructionBank.ClearInstructions();
        FlushRuntimeStorage();
        return true;
    }
    private static bool FlushRuntimeStorage()
    {
        if (IsRunning) return false;
        RegisterBank.ResetRegisters();
        SerialIO.FullFlush();
        // Memory.clear();
        return true;
    }
    private static bool Run()
    {
        if (IsRunning) return false;
        
        CanRun = true;
        ExecutionThread = new Thread(ExecutionLoop);
        ExecutionThread.Start();
        return true;
    }

    public static void ProceedStep()
    {
        CanGoToNextStep = true;
    }
    
    private static void ExecutionLoop()
    {
        IsRunning = true;
        OnRunStarted?.Invoke();
        
        while (CanRun)
        {
            if (SingleStep && !CanGoToNextStep) continue;
            ExecutionStep();
            if (InstructionPointer.Content >= InstructionBank.Count || InstructionPointer.Content < 0) break;
        }

        IsRunning = false;
        OnRunFinished?.Invoke();
    }
    private static void ExecutionStep()
    {
        if (InstructionBank.Count == 0) return;
        InstructionProcessor.ExecuteNextInstruction(InstructionPointer.Content);
        InstructionPointer.Content++;
        if (SingleStep) CanGoToNextStep = false;
    }
}