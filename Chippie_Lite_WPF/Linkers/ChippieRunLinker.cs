using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Instructions;
using Chippie_Lite_WPF.Computer.Internal;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
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
        Chippie.OnRunFinish += ChippieOnOnRunFinish;
    }

    public void RunScript(string script)
    {
        try
        {
            Owner.SetMode(AppMode.Run);
            Chippie.RunRawAssembly(script);
            Owner.ChangeDevAreaPage(2);
        }
        catch (InvalidInstructionException e)
        {
            Owner.SetMode(AppMode.Edit);
            Owner.ShowExceptionInCode(e);
        }
    }
    public void Halt()
    {
        Chippie.HaltOperation();
    }
    
    
    private void ChippieOnOnRunFinish()
    {
        Owner.SetMode(AppMode.Edit);
    }
    private void ChippieOnOnRunStarted()
    {
        
    }
}