using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Controls;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class SerialIOConsole : UserControl
{
    private SerialIOConsoleControl Control { get; set; }
    
    
    public SerialIOConsole()
    {
        InitializeComponent();
        Control = new SerialIOConsoleControl(this);
    }


    public void AddMessageToConsole(string msg)
    {
        if (msg.StartsWith('\n') && string.IsNullOrEmpty(GetConsoleText())) msg = msg[1..];
        RunDispatcher(() => IOConsole.Text += msg);
    }
    public string GetInputText()
    {
        string text = "";
        RunDispatcher(() => text = InputBox.Text);
        return text;
    }
    public void ClearInputBox()
    {
        RunDispatcher(() => InputBox.Text = "");
    }
    public void SetInputEnabled(bool enabled)
    {
        RunDispatcher(() => InputBox.IsEnabled = enabled);
    }
    private string GetConsoleText()
    {
        string text = "";
        RunDispatcher(() => text = IOConsole.Text);
        return text;
    }
    public void ClearConsole()
    {
        RunDispatcher(() => IOConsole.Text = "");
    }

    private void SubmitInputText()
    {
        if (string.IsNullOrEmpty(InputBox.Text)) return;
        string text = GetInputText();
        Control.SubmitInput(text);
    }
    
    
    private void SubmitBtn_OnClick(SquareButton sender)
    {
        SubmitInputText();
    }
    private void InputBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) SubmitInputText();
    }

    private void RunDispatcher(Action action) => Application.Current.Dispatcher.Invoke(action);
}