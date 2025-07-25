using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ConfirmationPrompt
{
    public bool ForceFocus { get; set; } = true;
    
    public delegate void ButtonSelectedAction(string choice);
    public event ButtonSelectedAction OnButtonSelected;
    
    public ConfirmationPrompt(string title, string body, IEnumerable<string>? buttons = null)
    {
        InitializeComponent();
        buttons ??= ["Cancel", "Ok"];
        SizeToContent = SizeToContent.WidthAndHeight;
        TitleBlock.Text = title;
        BodyBlock.Text = body;
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
    private void ButtonOnClick(SquareButton sender)
    {
        OnButtonSelected?.Invoke(sender.Label);
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