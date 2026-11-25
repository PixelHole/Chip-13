using Chippie_Lite_WPF.Computer.Assembly;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.File;
using Chippie_Lite_WPF.Computer.File.InstructionSets;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Utility;
using Chippie_Lite_WPF.UI.Elements;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.Computer;

public static class Chippie
{
    private static bool _singleStep = false;
    private static bool _isRunning = false;
    private static Instruction? lastInstruction;
    
    
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
            if (_singleStep == value) return;
            _singleStep = value;
            if (SingleStep) CanGoToNextStep = false;
            OnSingleStepChanged?.Invoke(SingleStep);
        } 
    }
    private static bool CanGoToNextStep { get; set; } = false;
    
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
        InstructionSetFileManager.LoadSets();
        SaveFileManager.LoadDefaultData();
    }
    public static void ShutdownProcedure()
    {
        HaltOperation();
        InstructionSetFileManager.SaveSets();
    }
    
    public static void CompileAndRunAssembly(string raw)
    {
        if (IsRunning) return;
        FullFlush();
        CompileAndLoadAssembly(raw);
        Run();
    }
    public static void Restart()
    {
        while (IsRunning) HaltOperation();
        Run();
    }
    public static void HaltOperation()
    {
        CanRun = false;
        SingleStep = true;
        CanGoToNextStep = false;
        FlushRuntimeStorage();
    }
    public static void SafeHalt()
    {
        SafeExit = true;
        CanRun = false;
    }
    
    private static bool CompileAndLoadAssembly(string raw)
    {
        if (IsRunning) return false;

        OnCompileStarted?.Invoke();
        
        var instructions = AssemblyTranslator.ParseScript(raw);
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
        IOBuffer.FullFlush();
        ChippieComponentManager.ResetNonPermanentMemoryModules();
    }
    private static void Run()
    {
        if (IsRunning) return;

        RegisterBank.InstructionPointer.ResetContent();

        SingleStep = StartSingleStep;
        CanRun = true;
        ExecutionThread = new Thread(ExecutionLoop) {
            Name = "Execution thread"
        };
        StepThread = new Thread(ExecutionStep) {
            Name = "Step thread"
        };
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
            if ((SingleStep && !CanGoToNextStep) || StepThread.IsAlive) continue;
            
            if (InstructionBank.Count == 0 ||
                RegisterBank.InstructionPointer.Content >= InstructionBank.Count ||
                RegisterBank.InstructionPointer.Content < 0) break;
            
            StepThread = new Thread(ExecutionStep);
            StepThread.Start();
        }

        if(StepThread.IsAlive && SafeExit) StepThread.Interrupt();
        
        if (!SafeExit) IsRunning = false;
    }
    private static void ExecutionStep()
    {
        Instruction instruction = InstructionBank.GetInstruction((int)RegisterBank.InstructionPointer.Content);

        if (instruction.HasBreakpoint && !SingleStep && lastInstruction != null && lastInstruction != instruction)
        {
            SingleStep = true;
            lastInstruction = instruction;
            return;
        }
        
        try
        {
            InstructionInterpreter.Interpret(instruction);
        }
        catch (ThreadInterruptedException)
        {
            if (SafeExit) return;
        }
        catch (Exception e)
        {
            HaltOperation();
            WPFUtility.DoUiAction(MainWindow.MainWindowIns, () => ErrorBox.Pop("Error running instruction", e.Message));
        }

        if (!IsRunning) return;
        
        lastInstruction = instruction;
        RegisterBank.InstructionPointer.Content++;
        if (SingleStep) CanGoToNextStep = false;
    }
}