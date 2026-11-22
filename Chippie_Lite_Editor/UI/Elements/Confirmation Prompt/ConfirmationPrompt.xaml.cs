using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.UI.Windows;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ConfirmationPrompt
{
    public bool ForceFocus { get; set; } = true;

    private Action<string>? ResultAction = null; 
    
    public ConfirmationPrompt(string title, string body, IEnumerable<string>? buttons = null, Action<string>? result = null)
    {
        InitializeComponent();
        Owner = MainWindow.MainWindowIns;
        Application.Current.Exit += (_, _) => Close();
        buttons ??= ["Cancel", "Ok"];
        SizeToContent = SizeToContent.WidthAndHeight;
        TitleBlock.Text = title;
        BodyBlock.Text = body;
        ResultAction = result;
        CreateButtons(buttons);
    }
    
    private void CreateButtons(IEnumerable<string> buttons)
    {
        ButtonsPanel.Children.Clear();
        
        foreach (var text in buttons)
        {
            SquareButton button = new SquareButton()
            {
                Label = text,
                Margin = new Thickness(2, 0, 2, 0)
            };

            button.Click += ButtonOnClick;
            
            ButtonsPanel.Children.Add(button);
        }
    }
    private void ButtonOnClick(SquareButton sender, MouseEventArgs e)
    {
        ResultAction?.Invoke(sender.Label);
        Close();
    }


    private void ConfirmationPrompt_OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (ForceFocus) Topmost = true;
    }
    private void DragBoxOnMouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}