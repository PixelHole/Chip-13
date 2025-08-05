using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;
using wpf_Console;
using Wpf_ConsoleIO;

namespace ConsoleControlLibrary;

public partial class ConsoleView : UserControl
{
    private bool initialized = false;
    private ConsoleControl Control;
    
    public Vector2Int GlyphCount { get; set; } = new Vector2Int(32, 16);
    public int GlyphCountX
    {
        get => GlyphCount.x;
        set => GlyphCount.SetX(value);
    }
    public int GlyphCountY
    {
        get => GlyphCount.y;
        set => GlyphCount.SetY(value);
    }
    private Vector2 GlyphAnchor { get; set; }
    
    
    public Size DisplaySize { get; private set; }
    private Size MinimumSize => new(GlyphCount.x, GlyphCount.y * 2);
    public Size GlyphSize => new(DisplaySize.Width / GlyphCount.x, DisplaySize.Height / GlyphCount.y);

    private ConsoleGlyph Caret;
    
    public ConsoleView()
    {
        InitializeComponent();
        SetupWindow();
        SetupCaret();
        ConnectEvents();
    }
    private void SetupWindow()
    {
        MinWidth = MinimumSize.Width;
        MinHeight = MinimumSize.Height;
        Control = new ConsoleControl(this);
    }
    private void SetupCaret()
    {
        if (CheckAccess())
            Caret = new ConsoleGlyph('I', 0, 7, Control.Cursor, ConsoleInputSource.User);
        else
            Dispatcher.Invoke(() => Caret = new ConsoleGlyph('I', 0, 7, Control.Cursor, ConsoleInputSource.User));
        
        Panel.SetZIndex(Caret, Int32.MaxValue);
        
        AddGlyph(Caret);
    }
    private void ConnectEvents()
    {
        Control.OnCursorMoved += ControlOnOnCursorMoved;
    }

    internal ConsoleGlyph CreateGlyph(char text, int foregroundIndex, int backgroundIndex, Vector2Int position, ConsoleInputSource source)
    {
        ConsoleGlyph? glyph = null;
        
        if (ConsoleCanvas.CheckAccess()) glyph = new ConsoleGlyph(text, foregroundIndex, backgroundIndex, position, source);
        else ConsoleCanvas.Dispatcher.Invoke(() => glyph = new ConsoleGlyph(text, foregroundIndex, backgroundIndex, position, source));
        AddGlyph(glyph);
        return glyph;
    }
    
    
    private void AddGlyph(ConsoleGlyph glyph)
    {
        UpdateGlyph(glyph);
        if (ConsoleCanvas.CheckAccess()) ConsoleCanvas.Children.Add(glyph);
        else ConsoleCanvas.Dispatcher.Invoke(() => ConsoleCanvas.Children.Add(glyph));
    }
    internal void RemoveGlyph(ConsoleGlyph glyph)
    {
        if (ConsoleCanvas.CheckAccess()) ConsoleCanvas.Children.Remove(glyph);
        else ConsoleCanvas.Dispatcher.Invoke(() => ConsoleCanvas.Children.Remove(glyph));
    }

    private void UpdateAllGlyphs()
    {
        foreach (var glyph in Control.Glyphs)
        {
            UpdateGlyph(glyph);
        }
        UpdateGlyph(Caret);
    }
    internal void UpdateGlyph(ConsoleGlyph glyph)
    {
        if (glyph.CheckAccess()) UpdateGlyphAction(glyph);
        else glyph.Dispatcher.Invoke(() => UpdateGlyphAction(glyph));
    }
    private void UpdateGlyphAction(ConsoleGlyph glyph)
    {
        var pos = ToCanvasPosition(glyph.Position.ToVector2());
        
        Canvas.SetLeft(glyph, pos.X);
        Canvas.SetTop(glyph, pos.Y);
        glyph.Width = GlyphSize.Width;
        glyph.Height = GlyphSize.Height;
        
        glyph.FontSize = double.Min(GlyphSize.Width, GlyphSize.Height) * 2;
    }
    
    private Vector2 ToCanvasPosition(Vector2 position)
    {
        return GlyphAnchor + new Vector2((float)(position.X * GlyphSize.Width), (float)(position.Y * GlyphSize.Height));
    }
    
    private void CalculateDisplaySize(Size controlSize)
    {
        double ratio;
        
        if (controlSize.Width > controlSize.Height) ratio = controlSize.Height / MinimumSize.Height;
        else ratio = controlSize.Width / MinimumSize.Width;
        
        DisplaySize = new Size(MinimumSize.Width * ratio, MinimumSize.Height * ratio);
    }
    private void CalculateNewAnchor(Size controlSize)
    {
        if (Math.Abs(controlSize.Height - controlSize.Width) < 0.1d)
        {
            GlyphAnchor = Vector2.Zero;
        }
        else if (controlSize.Width > controlSize.Height)
        {
            double x = controlSize.Width / 2 - DisplaySize.Width / 2;
            GlyphAnchor = new Vector2((float)x, 0);
        }
        else
        {
            double y = controlSize.Height / 2 - DisplaySize.Height / 2;
            GlyphAnchor = new Vector2(0, (float)y);
        }
    }
    
    private void ControlOnOnCursorMoved(Vector2Int position)
    {
        if (CheckAccess()) UpdateCaret(position);
        else Dispatcher.Invoke(() => UpdateCaret(position));
    }
    private void UpdateCaret(Vector2Int position)
    {
        Caret.Position = position;
        var glyph = Control.GetGlyphAt(position);

        Caret.Text = glyph?.Text ?? 'I';
        
        UpdateGlyph(Caret);
    }
    
    private void ConsoleView_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        CalculateDisplaySize(e.NewSize);
        CalculateNewAnchor(e.NewSize);
        UpdateAllGlyphs();
        Canvas.SetLeft(debugRect, GlyphAnchor.X);
        Canvas.SetTop(debugRect, GlyphAnchor.Y);
        debugRect.Width = DisplaySize.Width;
        debugRect.Height = DisplaySize.Height;
    }
    private void ConsoleView_OnKeyDown(object sender, KeyEventArgs e)
    {
        Control.ProcessUserKey(e.Key);
    }
    private void ConsoleView_OnTextInput(object sender, TextCompositionEventArgs e)
    {
        Control.ProcessUserText(e.Text);
    }
    private void ConsoleView_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (initialized) return;
        var window = Window.GetWindow(this);
        if (window == null) return;
        window.TextInput += ConsoleView_OnTextInput;
        window.KeyDown += ConsoleView_OnKeyDown;
        initialized = true;
    }
}