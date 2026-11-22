using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using Chippie_Lite_WPF.Computer.Utility;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ColorListEditItem : ColorsListItem
{
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(ColorListEditItem),
        new PropertyMetadata(false)
        );

    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }

    private ColorListEditItemControl Control;

    public ColorListEditItem()
    {
        InitializeComponent();
        Control = new ColorListEditItemControl(this);
    }
    
    protected override void ColorUpdated(Brush old, Brush newBrush)
    {
        ColorDisplay.Color = newBrush;
    }
    protected override void IndexUpdated(int index)
    {
        WPFUtility.SetContent(IndexLabel, index.ToString());
    }
    
    private void EditRequest()
    {
        Control.OpenColorWindow();
    }
    private void DeleteRequest()
    {
        ConfirmationPrompt prompt =
            new ConfirmationPrompt("Are you sure?", "Do you really want to delete this color?", ["Yes", "No"],
                s =>
                {
                    if (s == "Yes") ColorLibrary.RemoveColor(AssignedColor);
                });
        prompt.Show();
    }
    private void DuplicateRequest()
    {
        ColorLibrary.AddColor(AssignedColor);
    }
    
    private void ColorListEditItem_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        IsPressed = true;
    }
    private void ColorListEditItem_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        IsPressed = false;
    }
    private void ColorListEditItem_OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }
    
    private void EditBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        EditRequest();
    }
    private void DuplicateBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        DuplicateRequest();
    }
    private void DeleteBtn_OnClick(SquareButton sender, MouseButtonEventArgs e)
    {
        DeleteRequest();
    }
    private void ColorListEditItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        EditRequest();
    }
}