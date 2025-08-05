using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chippie_Lite_WPF.Controls;
using Chippie_Lite_WPF.UI.Elements;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class SerialIOConsole : UserControl
{
    private string inputText = "";
    private string consoleText = "";
    
    private SerialIOConsoleControl Control { get; set; }

    public string InputText
    {
        get => inputText;
        set
        {
            if (inputText == value) return;
            inputText = value;
            if (InputBox.CheckAccess()) InputBox.Text = inputText;
            else InputBox.Dispatcher.Invoke(() => InputBox.Text = value);
        }
    }

    public string ConsoleText
    {
        get => consoleText;
        set
        {
            if (consoleText == value) return;
            consoleText = value;
            if (IOConsole.CheckAccess()) IOConsole.Text = consoleText;
            else IOConsole.Dispatcher.Invoke(() => IOConsole.Text = consoleText);
        }
    }

    public bool IsInputEnabled
    {
        get
        {
            bool enabled = false;
            if (InputBox.CheckAccess()) enabled = InputBox.IsEnabled;
            else InputBox.Dispatcher.Invoke(() => enabled = InputBox.IsEnabled);
            return enabled;
        }
        set
        {
            if (InputBox.CheckAccess()) InputBox.IsEnabled = value;
            else InputBox.Dispatcher.Invoke(() => InputBox.IsEnabled = value);
        }
    }
    
    public SerialIOConsole()
    {
        InitializeComponent();
        Control = new SerialIOConsoleControl(this);
    }


    public void AddMessageToConsole(string msg)
    {
        if (msg.StartsWith('\n') && string.IsNullOrEmpty(ConsoleText)) msg = msg[1..];
        InputText += msg;
    }

    private void SubmitInputText()
    {
        if (string.IsNullOrEmpty(InputBox.Text)) return;
        Control.SubmitInput(inputText);
    }
    
    
    private void SubmitBtn_OnClick(SquareButton sender)
    {
        SubmitInputText();
    }
    private void InputBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) SubmitInputText();
    }
}