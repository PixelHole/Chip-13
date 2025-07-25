using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class RegisterListDisplayItem : UserControl
{
    private bool mouseInside = false;
    private bool mouseDown = false;

    private int displayedContent;
    private int displayFormat;
    
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

    public int DisplayFormat
    {
        get => displayFormat;
        set
        {
            displayFormat = value;
            UpdateContentDisplay();
        }
    }
    
    
    public RegisterListDisplayItem(Register register)
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
        IdLabel.Content = register.Id;
        DisplayedContent = register.Content;
        register.OnContentChanged += OnRegisterChanged;
    }

    private void OnRegisterChanged(int newValue)
    {
        displayedContent = newValue;
        UpdateContentDisplay();
    }
    private void UpdateContentDisplay()
    {
        var text = displayFormat switch
        {
            1 => NumberUtility.ToBinary(DisplayedContent, true),
            2 => NumberUtility.ToHex(DisplayedContent, true),
            _ => DisplayedContent.ToString()
        };

        if (Dispatcher.CheckAccess())
        {
            ContentLabel.Text = text;
            return;
        }

        Dispatcher.Invoke(() => ContentLabel.Text = text);
    }

    private void IncrementDisplayMode()
    {
        DisplayFormat++;
        if (DisplayFormat > 2) DisplayFormat = 0;
    }
    private void SetColor(Brush color)
    {
        Body.Background = color;
    }
    private void UpdateColor()
    {
        if (mouseInside)
        {
            SetColor(mouseDown ? ClickColor : HoverColor);
            return;
        }
        SetColor(DefaultColor);
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

        switch (e.ChangedButton)
        {
            case MouseButton.Left:
                IncrementDisplayMode();
                break;
            case MouseButton.Right:
                Clipboard.SetText(ContentLabel.Text.ToString()!);
                break;
        }
    }
}