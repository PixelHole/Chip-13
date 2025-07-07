using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class VideoIO : UserControl
{
    public int DisplayWidth { get; private set; } = 200;
    public int DisplayHeight { get; private set; } = 150;
    public int PixelSize { get; private set; } = 4;
    public Brush PixelDefaultColor { get; private set; }

    private Rectangle[,] Pixels { get; set; }
    
    
    public VideoIO()
    {
        InitializeComponent();
        FetchColors();
        CreatePixels();
        DebugColorPixels();
    }
    private void FetchColors()
    {
        PixelDefaultColor = (Application.Current.Resources["Black"] as Brush)!;
    }

    private void CreatePixels()
    {
        Pixels = new Rectangle[DisplayWidth, DisplayHeight];
        for (int y = 0; y < DisplayHeight; y++)
        {
            for (int x = 0; x < DisplayWidth; x++)
            {
                Rectangle pixel = new Rectangle()
                {
                    Width = PixelSize,
                    Height = PixelSize,
                    Fill = PixelDefaultColor,
                };
                Pixels[x, y] = pixel;
                Display.Children.Add(pixel);
                Canvas.SetTop(pixel, y * PixelSize);
                Canvas.SetLeft(pixel, x * PixelSize);
            }
        }
    }
    private void DebugColorPixels()
    {
        for (int y = 0; y < DisplayHeight; y++)
        {
            for (int x = 0; x < DisplayWidth; x++)
            {
                var pixel = Pixels[x, y];
                byte red = (byte)((float)x / DisplayWidth * 255f);
                byte blue = (byte)((float)y / DisplayHeight * 255f);

                pixel.Fill = new SolidColorBrush(Color.FromRgb(0, red, blue));
            }
        }
    }
}