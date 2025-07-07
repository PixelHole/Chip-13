using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Instructions.Templates;
using Chippie_Lite_WPF.Computer.Internal;

namespace Chippie_Lite_WPF.Computer;

public static class Chippie
{
    private static bool _singleStep = true;
    private static bool _isRunning = false;
    
    public static bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            if (IsRunning) OnRunStarted?.Invoke();
            else OnRunFinished?.Invoke();
        }
        
    }
    public static bool CanRun { get; private set; } = true;
    public static bool SingleStep
    {
        get => _singleStep;
        set
        {
            _singleStep = value;
            OnSingleStepChanged?.Invoke(SingleStep);
        } 
    }
    private static bool CanGoToNextStep { get; set; } = false;
    public static Register InstructionPointer { get; private set; }
    private static Thread ExecutionThread { get; set; }
    private static Thread StepThread { get; set; }


    public delegate void CompileStartAction();
    public static event CompileStartAction OnCompileStarted;
    
    public delegate void CompileEndAction();
    public static event CompileEndAction OnCompileEnded;
    
    public delegate void RunFinishedAction();
    public static event RunFinishedAction OnRunFinished;

    public delegate void RunStartAction();
    public static event RunStartAction OnRunStarted;

    public delegate void SingleStepChangeAction(bool enabled);
    public static event SingleStepChangeAction OnSingleStepChanged;

    public static void Initialize()
    {
        InstructionPointer = RegisterBank.GetRegister("Instruction Pointer")!;
        InstructionSet.LoadSet(InstructionSet.SavePath);
    }
    
    public static void RunRawAssembly(string raw)
    {
        if (IsRunning) return;
        FullFlush();
        CompileAndLoadAssembly(raw);
        Run();
    }
    public static void Restart()
    {
        while (IsRunning) HaltOperation();
        FlushRuntimeStorage();
        Run();
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
        StepThread = new Thread(ExecutionStep);
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
        
        while (CanRun)
        {
            if (InstructionBank.Count == 0 || InstructionPointer.Content >= InstructionBank.Count || InstructionPointer.Content < 0) break;
            
            if ((SingleStep && !CanGoToNextStep) || StepThread.IsAlive) continue;
            
            StepThread = new Thread(ExecutionStep);
            StepThread.Start();
        }

        IsRunning = false;
    }
    private static void ExecutionStep()
    {
        InstructionProcessor.ExecuteNextInstruction(InstructionPointer.Content);
        InstructionPointer.Content++;
        if (SingleStep) CanGoToNextStep = false;
    }
}