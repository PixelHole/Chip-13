using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.Linkers;

public class MainWindowControl
{
    private MainWindow Owner { get; set; }

    public MainWindowControl(MainWindow owner)
    {
        Owner = owner;
        ConnectEvents();
    }

    protected void ConnectEvents()
    {
        Chippie.OnRunStarted += ChippieOnOnRunStarted;
        Chippie.OnRunFinished += ChippieOnOnRunFinished;
        Chippie.OnSingleStepChanged += OnSingleChanged;
    }

    public void RunScript(string script)
    {
        try
        {
            Chippie.RunRawAssembly(script);
        }
        catch (Exception e)
        {
            Chippie.HaltOperation();

            switch (e)
            {
                case InvalidInstructionException parseException:
                    Owner.ShowExceptionInCode(parseException);
                    break;

                case InstructionInterpretationException interpretationException:
                    Owner.ShowExceptionInCode(interpretationException);
                    break;

                default:
                    Owner.ShowError("internal error", $"An internal error occured : {e.Message}");
                    break;
            }
        }
    }
    public void Halt()
    {
        Chippie.HaltOperation();
    }
    public void Restart()
    {
        Chippie.Restart();
    }
    public void NextStep()
    {
        Chippie.ProceedStep();
    }
    public void RunToEnd()
    {
        Chippie.SingleStep = false;
    }
    
    public void SaveInstance()
    {
        
    }
    public void LoadInstance()
    {
        
    }
    public void NewInstance()
    {
        
    }

    private void OnSingleChanged(bool singleStep)
    {
        Owner.RunToEndButtonVisible(singleStep);
    }

    private void ChippieOnOnRunFinished()
    {
        Owner.SetMode(AppMode.Edit);
    }
    private void ChippieOnOnRunStarted()
    {
        Owner.SetMode(AppMode.Run);
        Owner.ChangeDevAreaPage(2);
    }
}