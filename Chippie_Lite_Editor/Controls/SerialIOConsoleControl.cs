using Chippie_Lite_WPF.Computer;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.UI.Windows.Development;

namespace Chippie_Lite_WPF.Controls;

public class SerialIOConsoleControl
{
    public SerialIOConsole Owner { get; private set; }
    private int LastSource { get; set; } = -1;
    
    
    
    public SerialIOConsoleControl(SerialIOConsole owner)
    {
        Owner = owner;
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        SerialIO.OnOutputBuffered += OnOutputBuffered;
        Chippie.OnRunStarted += OnRunStart;
        Chippie.OnRunFinished += OnRunEnd;
    }

    private void OnOutputBuffered()
    {
        string output = SerialIO.GetOutput();
        AddChippieTextToConsole(output);
    }
    public void SubmitInput(string input)
    {
        Owner.SetInputEnabled(false);
        SerialIO.BufferInput(input);
        AddUserTextToConsole(input);
        Owner.ClearInputBox();
        Owner.SetInputEnabled(true);
    }

    private void AddUserTextToConsole(string text)
    {
        if (LastSource == 0) LastSource = -1;
        AddTextToConsole(text, 0);
    }
    private void AddChippieTextToConsole(string text) => AddTextToConsole(text, 1);
    private void AddTextToConsole(string text, int source)
    {
        if (source != LastSource)
        {
            LastSource = source;
            string header = LastSource == 0 ? "@USER" : "@CHIP-13";
            Owner.AddMessageToConsole($"\n{header} : ");
        }
        
        Owner.AddMessageToConsole(text);
    }

    private void OnRunStart()
    {
        Owner.ClearConsole();
        Owner.SetInputEnabled(true);
    }
    private void OnRunEnd()
    {
        LastSource = -1;
        Owner.SetInputEnabled(false);
    }
}