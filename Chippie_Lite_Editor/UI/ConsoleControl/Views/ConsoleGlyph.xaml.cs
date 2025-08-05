using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_Console;

public partial class ConsoleGlyph : UserControl
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
    public new Brush Foreground
    {
        get
        {
            if (GlyphText.CheckAccess())
            {
                return GlyphText.Foreground;
            }

            Brush foreground = null!;
            GlyphText.Dispatcher.Invoke(() => foreground = GlyphText.Foreground);
            return foreground!;
        }
        set
        {
            if (GlyphText.CheckAccess())
            {
                GlyphText.Foreground = value;
                return;
            }
            GlyphText.Dispatcher.Invoke(() => GlyphText.Foreground = value);
        }
    }
    public new Brush Background
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
            if (Body.CheckAccess())
            {
                Body.Background = value;
                return;
            }
            GlyphText.Dispatcher.Invoke(() => Body.Background = value);
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
    
    public ConsoleInputSource Source { get; set; }
    public bool Processed { get; set; } = false;
    
    public Vector2Int Position { get; set; } = Vector2Int.Zero;


    public ConsoleGlyph(char text, Brush foreground, Brush background, Vector2Int position, ConsoleInputSource source)
    {
        InitializeComponent();
        Text = text;
        Position = position;
        Foreground = foreground;
        Background = background;
        Source = source;
    }


    public override string? ToString()
    {
        return $"{Text}({Position})";
    }
}