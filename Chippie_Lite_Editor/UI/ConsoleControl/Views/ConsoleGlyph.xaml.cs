using System.Windows.Controls;
using System.Windows.Media;
using Chippie_Lite_WPF.Computer.Components;

namespace wpf_Console;

public partial class ConsoleGlyph
{
    private char text = '\0';
    
    public new double FontSize
    {
        get
        {
            if (GlyphText.CheckAccess())
            {
                return GlyphText.FontSize;
            }

            double size = 8;
            GlyphText.Dispatcher.Invoke(() => size = GlyphText.FontSize);
            return size;
        }
        set
        {
            if (GlyphText.CheckAccess())
            {
                GlyphText.FontSize = value > 8 ? value : 8;
                return;
            }

            GlyphText.Dispatcher.Invoke(() => GlyphText.FontSize = value > 8 ? value : 8);
        }
    }
    public new FontFamily FontFamily
    {
        get
        {
            if (GlyphText.CheckAccess())
            {
                return GlyphText.FontFamily;
            }

            FontFamily font = null!;
            GlyphText.Dispatcher.Invoke(() => font = GlyphText.FontFamily);
            return font!;
        }
        set
        {
            if (GlyphText.CheckAccess())
            {
                GlyphText.FontFamily = value;
                return;
            }

            GlyphText.Dispatcher.Invoke(() => GlyphText.FontFamily = value);
        }
    }
    private new Brush Foreground
    {
        get
        {
            if (GlyphText.CheckAccess())
            {
                return GlyphText.Foreground;
            }

            Brush foreground = null!;
            GlyphText.Dispatcher.Invoke(() => foreground = GlyphText.Foreground);
            return foreground;
        }
        set
        {
            if (CheckAccess()) GlyphText.Foreground = value;
            else Dispatcher.Invoke(() => GlyphText.Foreground = value);
        }
    }
    private new Brush Background
    {
        get
        {
            if (Body.CheckAccess())
            {
                return Body.Background;
            }

            Brush background = null!;
            GlyphText.Dispatcher.Invoke(() => background = Body.Background);
            return background!;
        }
        set
        {
            if (CheckAccess()) Body.Background = value;
            else Dispatcher.Invoke(() => Body.Background = value);
        }
    }
    public char Text
    {
        get => text;
        set
        {
            bool changed = text != value;
            if (!changed) return;

            text = value;
            
            if (GlyphText.CheckAccess())
            {
                GlyphText.Text = value.ToString();
                return;
            }

            GlyphText.Dispatcher.Invoke(() => GlyphText.Text = value.ToString());
        }
    }
    
    public ConsoleInputSource Source { get; }
    public bool Processed { get; set; }
    
    public Vector2Int Position { get; set; }


    public ConsoleGlyph(char text, int foregroundIndex, int backgroundIndex, Vector2Int position, ConsoleInputSource source)
    {
        InitializeComponent();
        Text = text;
        Position = position;
        SetForeground(foregroundIndex);
        SetBackground(backgroundIndex);
        Source = source;
    }


    public void SetForeground(int index)
    {
        Foreground = new SolidColorBrush(ColorLibrary.GetColor(index));
    }
    public void SetBackground(int index)
    {
        Background = new SolidColorBrush(ColorLibrary.GetColor(index));
    }
    
    public override string ToString()
    {
        return $"{Text}({Position})";
    }
}