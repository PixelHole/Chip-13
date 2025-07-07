using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;

namespace Chippie_Lite_WPF.UI.Elements.RegisterDisplay;

public partial class RegisterListItem : UserControl
{
    private bool mouseInside;
    private bool mouseDown;

    private int displayedContent;
    private int displayMode;
    
    public Brush DefaultColor { get; set; } = new SolidColorBrush(Colors.Transparent);
    public Brush HoverColor { get; set; }
    public Brush ClickColor { get; set; }

    public int DisplayedContent
    {
        get => displayedContent;
        set
        {
            displayedContent = value;
            UpdateContentDisplay();
        }
    }

    public int DisplayMode
    {
        get => displayMode;
        set
        {
            displayMode = value;
            UpdateContentDisplay();
        }
    }
    
    
    public RegisterListItem(Register register)
    {
        InitializeComponent();
        FetchColors();
        ConnectToRegister(register);
        UpdateContentDisplay();
    }
    private void FetchColors()
    {
        HoverColor = (Application.Current.Resources["Faded Purple"] as Brush)!;
        ClickColor = (Application.Current.Resources["Pink"] as Brush)!;
    }
    private void ConnectToRegister(Register register)
    {
        register.OnContentChanged += OnRegisterChanged;
        IdLabel.Content = register.Id;
        DisplayedContent = register.Content;
    }

    private void OnRegisterChanged(int newValue)
    {
        displayedContent = newValue;
        try
        {
            Application.Current.Dispatcher.Invoke(UpdateContentDisplay);
        }
        catch (Exception)
        {
            // ignored
        }
    }
    private void UpdateContentDisplay()
    {
        string text = displayMode switch
        {
            1 => DisplayContentToBinary(),
            2 => DisplayContentToHex(),
            _ => DisplayedContent.ToString()
        };

        ContentLabel.Text = text;
    }

    private void IncrementDisplayMode()
    {
        DisplayMode++;
        if (DisplayMode > 2) DisplayMode = 0;
    }
    private void SetColor(Brush color)
    {
        Body.Background = color;
    }
    private void UpdateColor()
    {
        switch (mouseDown)
        {
            case true :
                SetColor(mouseInside ? ClickColor : HoverColor);
                break;
            case false :
                SetColor(mouseInside ? HoverColor : DefaultColor);
                break;
        }
    }

    private string DisplayContentToBinary()
    {
        string raw = Convert.ToString(DisplayedContent, 2).PadLeft(32, '0');
        return $"{raw.Substring(0, 8)}-{raw.Substring(8, 8)}-{raw.Substring(16, 8)}-{raw.Substring(24, 8)}b";
    }
    private string DisplayContentToHex()
    {
        string raw = Convert.ToString(DisplayedContent, 16).PadLeft(8, '0');
        return $"0x{raw.Substring(0, 4)}-{raw.Substring(4, 4)}";
    }
    
    private void Body_OnMouseEnter(object sender, MouseEventArgs e)
    {
        mouseInside = true;
        UpdateColor();
    }
    private void Body_OnMouseLeave(object sender, MouseEventArgs e)
    {
        mouseInside = false;
        UpdateColor();
    }
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        mouseDown = true;
        UpdateColor();
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        mouseDown = false;
        UpdateColor();
        
        if (e.ChangedButton == MouseButton.Left)
        {
            IncrementDisplayMode();
        }
        else if (e.ChangedButton == MouseButton.Right)
        {
            Clipboard.SetText(ContentLabel.Text);
        }
    }
}