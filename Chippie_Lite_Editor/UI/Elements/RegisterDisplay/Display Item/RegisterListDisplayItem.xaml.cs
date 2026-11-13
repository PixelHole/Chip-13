using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class RegisterListDisplayItem
{
    public static readonly DependencyProperty DefaultColorProperty = DependencyProperty.Register(
        nameof(DefaultColor),
        typeof(Brush),
        typeof(RegisterListDisplayItem),
        new PropertyMetadata(new SolidColorBrush(Colors.Transparent))
        );
    public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(
        nameof(HoverColor),
        typeof(Brush),
        typeof(RegisterListDisplayItem),
        new PropertyMetadata(Application.Current.Resources["Faded Purple"] as Brush)
    );
    public static readonly DependencyProperty ClickColorProperty = DependencyProperty.Register(
        nameof(ClickColor),
        typeof(Brush),
        typeof(RegisterListDisplayItem),
        new PropertyMetadata(Application.Current.Resources["Pink"] as Brush)
    );
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(RegisterListDisplayItem),
        new PropertyMetadata(false)
    );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }

    private long displayedContent;
    private int displayFormat;
    
    public Brush DefaultColor {
        get => (Brush)GetValue(DefaultColorProperty);
        set => SetValue(DefaultColorProperty, value);
    }
    public Brush HoverColor {
        get => (Brush)GetValue(HoverColorProperty);
        set => SetValue(HoverColorProperty, value);
    }
    public Brush ClickColor {
        get => (Brush)GetValue(ClickColorProperty);
        set => SetValue(ClickColorProperty, value);
    }

    public long DisplayedContent
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

    private byte RegisterLength { get; set; }
    
    
    public RegisterListDisplayItem()
    {
        InitializeComponent();
        UpdateContentDisplay();
    }
    protected override void LoadRegisterData()
    {
        WPFUtility.SetContent(IdLabel, AssignedRegister?.Id ?? "Id");
        DisplayedContent = AssignedRegister?.Content ?? 0;
        if (AssignedRegister != null) AssignedRegister.OnContentChanged += OnRegisterChanged;
    }

    private void OnRegisterChanged(long newValue)
    {
        displayedContent = newValue;
        UpdateContentDisplay();
    }
    private void UpdateContentDisplay()
    {
        var text = displayFormat switch
        {
            1 => NumberUtility.ToBinary(DisplayedContent, RegisterLength, true),
            2 => NumberUtility.ToHex(DisplayedContent, RegisterLength, true),
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
    
    private void Body_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void Body_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;

        switch (e.ChangedButton)
        {
            case MouseButton.Left:
                IncrementDisplayMode();
                break;
            case MouseButton.Right:
                Clipboard.SetText(ContentLabel.Text);
                break;
        }
    }
}