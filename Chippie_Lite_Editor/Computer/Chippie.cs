using Chippie_Lite_WPF.Computer.Assembly;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;

namespace Chippie_Lite_WPF.Computer;

public static class Chippie
{
    private static bool _singleStep = false;
    private static bool _isRunning = false;

    private static bool SafeExit { get; set; } = false;
    
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
    public static bool StartSingleStep { get; set; } = true;
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
    public static Register InstructionPointer { get; private set; } = null!;
    private static Thread ExecutionThread { get; set; } = null!;
    private static Thread StepThread { get; set; } = null!;
    

    public delegate void CompileStartAction();
    public static event CompileStartAction? OnCompileStarted;
    
    public delegate void CompileEndAction();
    public static event CompileEndAction? OnCompileEnded;
    
    public delegate void RunFinishedAction();
    public static event RunFinishedAction? OnRunFinished;

    public delegate void RunStartAction();
    public static event RunStartAction? OnRunStarted;

    public delegate void SingleStepChangeAction(bool enabled);
    public static event SingleStepChangeAction? OnSingleStepChanged;

    public static void Initialize()
    {
        InstructionPointer = RegisterBank.GetRegister("Instruction Pointer") ??
                             throw new RegisterNotFoundException(0, "Instruction Pointer");
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
    public static void HaltOperation()
    {
        CanRun = false;
    }
    public static void SafeHalt()
    {
        SafeExit = true;
        CanRun = false;
        
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
    public static void FullFlush()
    {
        if (IsRunning) return;
        InstructionBank.ClearInstructions();
        FlushRuntimeStorage();
    }
    private static void FlushRuntimeStorage()
    {
        if (IsRunning) return;
        RegisterBank.ResetRegisters();
        SerialIO.FullFlush();
        Memory.CopyInitialStorageToRuntime();
    }
    private static void Run()
    {
        if (IsRunning) return;

        SingleStep = StartSingleStep;
        CanRun = true;
        ExecutionThread = new Thread(ExecutionLoop);
        ExecutionThread.Name = "Execution thread";
        StepThread = new Thread(ExecutionStep);
        StepThread.Name = "Step thread";
        ExecutionThread.Start();
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

        if(StepThread.IsAlive && SafeExit) StepThread.Interrupt();
        
        if (!SafeExit) IsRunning = false;
    }
    private static void ExecutionStep()
    {
        try
        {
            InstructionProcessor.ExecuteNextInstruction(InstructionPointer.Content);
        }
        catch (ThreadInterruptedException)
        {
            if (SafeExit) return;
        }
        InstructionPointer.Content++;
        if (SingleStep) CanGoToNextStep = false;
    }
}