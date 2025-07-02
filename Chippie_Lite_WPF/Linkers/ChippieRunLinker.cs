using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.Linkers;

public class ChippieRunLinker
{
    private MainWindow Owner { get; set; }


    public ChippieRunLinker(MainWindow owner)
    {
        Owner = owner;
        ConnectEvents();
    }

    private void ConnectEvents()
    {
        Chippie.OnRunStarted += ChippieOnOnRunStarted;
        Chippie.OnRunFinished += ChippieOnOnRunFinished;
    }

    public void RunScript(string script)
    {
        try
        {
            Owner.SetMode(AppMode.Run);
            Chippie.RunRawAssembly(script);
            Owner.ChangeDevAreaPage(2);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case InvalidInstructionException parseException :
                    Owner.SetMode(AppMode.Edit);
                    Owner.ShowExceptionInCode(parseException);
                    break;
                
                case InstructionInterpretationException interpretationException :
                    Owner.SetMode(AppMode.Edit);
                    Owner.ShowExceptionInCode(interpretationException);
                    break;
            }
        }
    }
    public void Halt()
    {
        Chippie.HaltOperation();
    }
    
    
    private void ChippieOnOnRunFinished()
    {
        Owner.SetMode(AppMode.Edit);
    }
    private void ChippieOnOnRunStarted()
    {
        
    }
}