using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using Chippie_Lite_WPF.Computer.Internal.Exceptions;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Base;
using Chippie_Lite_WPF.Computer.Internal.Exceptions.Interpretation;
using Chippie_Lite_WPF.UI.Elements;
using Vector = System.Windows.Vector;

namespace Chippie_Lite_WPF.UI.Windows;

public partial class DevelopmentArea : UserControl
{
    public bool SourceChanged { get; set; } = false;
    
    
    public DevelopmentArea()
    {
        InitializeComponent();
        ConnectEvents();
    }
    private void ConnectEvents()
    {
        CodeEditControl.InputBox.TextChanged += InputBoxOnTextChanged;
    }
    

    public void SetMode(AppMode mode)
    {
        switch (mode)
        {
            case AppMode.Run:
                RunModeProcedure();
                break;

            case AppMode.Edit:
                EditModeProcedure();
                break;
        }
    }

    private void EditModeProcedure()
    {
        CodeEditControl.SetEditable(true);
    }
    private void RunModeProcedure()
    {
        CodeEditControl.SetEditable(false);
    }
    
    
    // code edit window
    public string GetInputCode()
    {
        return CodeEditControl.InputText;
    }
    public void SetInputCode(string code)
    {
        CodeEditControl.InputText = code;
    }
    public Point HighlightCodeLine(int line)
    {
        var point = CodeEditControl.HighlightLine(line);

        point = TranslatePoint(point, CodeEditControl);
        
        var window = Application.Current.MainWindow;
        
        point += new Vector(window.Left, window.Top);
        
        return point;
    }
    
    public void ChangePage(int index)
    {
        switch (index)
        {
            case 0 :
                CodeEditControl.Visibility = Visibility.Visible;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
            
            case 1 :
                CodeEditControl.Visibility = Visibility.Collapsed;
                MemoryEditControl.Visibility = Visibility.Visible;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
            
            case 2 :
                CodeEditControl.Visibility = Visibility.Collapsed;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Visible;
                break;
            
            default :
                CodeEditControl.Visibility = Visibility.Visible;
                MemoryEditControl.Visibility = Visibility.Collapsed;
                RunWindow.Visibility = Visibility.Collapsed;
                break;
        }
    }
    
    private void InputBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        SourceChanged = true;
    }
}